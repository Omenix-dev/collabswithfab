using AutoMapper;
using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Models.DTO.Request.Enums;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services.Abstract.DashBoardInterfaces;
using MedicalRecordsApi.Services.Common.Interfaces;
using MedicalRecordsData.DatabaseContext;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsData.Enum;
using MedicalRecordsRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Implementation.DashBoardServices
{
    public class DashBoardService : IDashBoardService
    {
        #region config
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IGenericRepository<PatientAssignmentHistory> _patientAssignmentHistoryRepository;
        private readonly IGenericRepository<AssignPatientBed> _assignPatientBedRepository;

        public DashBoardService(IGenericRepository<Patient> patientRepository, IGenericRepository<Employee> employeeRepository,
            IGenericRepository<PatientAssignmentHistory> patientAssignmentHistoryRepository, IGenericRepository<AssignPatientBed> assignPatientBedRepository)
        {
            _patientRepository = patientRepository;
            _employeeRepository = employeeRepository;
            _patientAssignmentHistoryRepository = patientAssignmentHistoryRepository;
            _assignPatientBedRepository = assignPatientBedRepository;
        }
        #endregion

        public async Task<ServiceResponse<long>> GetAssignedPatientsCountAsync(int userId, PatientCareStatus status)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<long>(0, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            long patientCount = 0;

            if (status == PatientCareStatus.All)
            {
                patientCount = await _patientAssignmentHistoryRepository.Query()
                                                      .AsNoTracking()
                                                      .Where(x => x.DoctorId == userId 
                                                       || x.NurseId == userId)
                                                      .Select(x => x.PatientId)
                                                      .Distinct().CountAsync();
            }
            else if (status == PatientCareStatus.Waiting)
            {
                patientCount = await _patientRepository.Query()
                                                      .AsNoTracking()
                                                      .Where(x => x.DoctorId == userId || x.NurseId == userId).CountAsync();
            }

            return new ServiceResponse<long>(patientCount, InternalCode.Success);
        }
        public async Task<ServiceResponse<long>> GetAdmittedPatientsCountAsync(int userId)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<long>(0, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var bedsAssigned = _assignPatientBedRepository.Query()
                                                       .AsNoTracking().ToList();

            if (!bedsAssigned.Any())
            {
                return new ServiceResponse<long>(0, InternalCode.Success);
            }


            var patients = await _patientRepository.Query()
                                                  .AsNoTracking()
                                                  .Where(x => x.DoctorId == userId).ToListAsync();

            if (!patients.Any())
            {
                return new ServiceResponse<long>(0, InternalCode.Success);
            }

            HashSet<int> patientIds = new HashSet<int>(bedsAssigned.Select(x => x.PatientAssignedId).ToList());
            IEnumerable<int> patientCount = patients.Select(x => x.Id).ToList().Where(patientIds.Contains);

            return new ServiceResponse<long>(patientCount.Count(), InternalCode.Success);
        }
        public async Task<ServiceResponse<long>> GetInPatientOutPatientPatientsCountAsync(int userId, PatientCareType careType)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<long>(0, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            long patientCount = patientCount = await _patientAssignmentHistoryRepository.Query()
                                                      .AsNoTracking()
                                                      .Where(x => x.DoctorId == userId && x.CareType == careType)
                                                      .Select(x => x.PatientId)
                                                      .Distinct().CountAsync();
            
            return new ServiceResponse<long>(patientCount, InternalCode.Success);
        }
        public async Task<ServiceResponse<ReadPatientCareTypeDto>> InPatientOutPatientDataAndPercentagesAsync(int userId)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<ReadPatientCareTypeDto>(null, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var patientAssignment = await _patientAssignmentHistoryRepository.Query().AsNoTracking()
                                                            .Where(history => history.DoctorId == userId)
                                                            .ToListAsync();

            if (!patientAssignment.Any())
            {
                return new ServiceResponse<ReadPatientCareTypeDto>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            var totalCount = patientAssignment.Count();
            var careCount = await _patientAssignmentHistoryRepository.Query().AsNoTracking()
                                                                     .Where(history => history.DoctorId == userId)
                                                                     .GroupBy(history => history.CareType)
                                                                     .Select(group => new ReadPatientCareTypeDto
                                                                     {
                                                                         InPatientPercentage = group.Key == PatientCareType.InPatient ? (double)group.Count() / totalCount * 100 : 0,
                                                                         OutPatientPercentage = group.Key == PatientCareType.OutPatient ? (double)group.Count() / totalCount * 100 : 0
                                                                     })
                                                                     .FirstOrDefaultAsync();

            if (careCount == null)
            {
                return new ServiceResponse<ReadPatientCareTypeDto>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            var average = patientAssignment.GroupBy(history => history.CreatedAt);

            foreach (var item in average.OrderBy(x => x.Key))
            {
                DailyAverageCount dailyAverageCount = new DailyAverageCount();
                dailyAverageCount.Date = item.Key.ToString("MMM dd");
                dailyAverageCount.InPatientCount = item.Count(x => x.CareType == PatientCareType.InPatient);
                dailyAverageCount.OutPatientCount = item.Count(x => x.CareType == PatientCareType.OutPatient);


                careCount.DailyAverageCount.Add(dailyAverageCount);
            }

            return new ServiceResponse<ReadPatientCareTypeDto>(careCount, InternalCode.Success);
        }
        public async Task<ServiceResponse<ReadPatientByGenderDto>> PatientByGenderAsync(int userId)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<ReadPatientByGenderDto>(null, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            int totalCount = await _patientRepository.Query().AsNoTracking()
                                                     .Where(history => history.DoctorId == userId)
                                                     .CountAsync();

            var careCount = await _patientRepository.Query().AsNoTracking()
                                                    .Where(history => history.DoctorId == userId)
                                                    .GroupBy(history => history.Gender)
                                                    .Select(group => new ReadPatientByGenderDto
                                                    {
                                                        MalePatientPercentage = group.Key.ToLower() == "male" ? (double)group.Count() / totalCount * 100 : 0,
                                                        FemalePatientPercentage = group.Key.ToLower() == "female" ? (double)group.Count() / totalCount * 100 : 0
                                                    })
                                                    .FirstOrDefaultAsync();

            if (careCount == null)
            {
                return new ServiceResponse<ReadPatientByGenderDto>(null, InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            return new ServiceResponse<ReadPatientByGenderDto>(careCount, InternalCode.Success);
        }
        public async Task<ServiceResponse<IEnumerable<ReadPatientAdmissionDto>>> PatientAdmissionAsync(int userId)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<IEnumerable<ReadPatientAdmissionDto>>(Enumerable.Empty<ReadPatientAdmissionDto>(), InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            var patientAdmission = await _patientRepository.Query().AsNoTracking()
                                                     .Where(history => history.DoctorId == userId && history.CreatedAt > DateTime.Now.AddDays(-1))
                                                     .GroupBy(admission => admission.CreatedAt.Hour)
                                                     .Select(group => new ReadPatientAdmissionDto
                                                     {
                                                         Time = $"{group.Key}:00 - {group.Key + 1}:00",
                                                         Count = group.Count()
                                                     })
                                                     .ToListAsync();

            if (!patientAdmission.Any())
            {
                return new ServiceResponse<IEnumerable<ReadPatientAdmissionDto>>(Enumerable.Empty<ReadPatientAdmissionDto>(), InternalCode.EntityNotFound, ServiceErrorMessages.EntityNotFound);
            }

            return new ServiceResponse<IEnumerable<ReadPatientAdmissionDto>>(patientAdmission, InternalCode.Success);
        }
        public async Task<ServiceResponse<long>> GetPatientByHmoAsync(int userId)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<long>(0, InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }

            long patientCount = patientCount = await _patientRepository.Query()
                                                      .AsNoTracking()
                                                      .Where(x => x.DoctorId == userId && x.HasHmo == true).CountAsync();

            return new ServiceResponse<long>(patientCount, InternalCode.Success);
        }

        public ServiceResponse<object> AvaliableStaff(int ClinicId)
        {
            try
            {
                var NurseAndDoctorData = _employeeRepository.GetAll().Where(x => (x.RoleId == (int)MedicalRole.Nurse || x.RoleId == (int)MedicalRole.Doctors) && x.ClinicId == ClinicId).Select(x => x.Id);
                var PatientsNurseData = _patientRepository.GetAll().Where(x => x.ClinicId == ClinicId).Select(x => x.NurseId);
                var PatientsDoctorData = _patientRepository.GetAll().Where(x => x.ClinicId == ClinicId).Select(x => x.DoctorId);
                var totalAvailable = NurseAndDoctorData.Count() - NurseAndDoctorData.Where(x => PatientsNurseData.Contains(x) || PatientsDoctorData.Contains(x)).Count();
                return new ServiceResponse<object>(new { Message = "Success", AvaliableStaff = totalAvailable }, InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<object>(null, InternalCode.Incompleted, ex.Message);
            }
        }
    }
}
