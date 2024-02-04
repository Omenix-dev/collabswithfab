using AutoMapper;
using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Services.Abstract.ReferralInterfaces;
using MedicalRecordsData.DatabaseContext;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.DTO.ReferralDto;
using MedicalRecordsRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Implementation.ReferralServices
{

    public class ReferralServices : IReferralServices
    {
        private readonly IMapper _mapper;
        private readonly MedicalRecordDbContext _dbContext;
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IGenericRepository<Contact> _contactRepository;
        private readonly IGenericRepository<EmergencyContact> _emrgencyContactRepository;
        private readonly IGenericRepository<Immunization> _immunizationRepository;
        private readonly IGenericRepository<ImmunizationDocument> _immunizationDocumentRepository;
        private readonly IGenericRepository<MedicalRecord> _medicalRecordRepository;
        private readonly IGenericRepository<Medication> _medicationRepository;
        private readonly IGenericRepository<PatientReferrer> _patientReferrerRepository;
        private readonly IConfiguration _configuration;
        public ReferralServices()
        {

        }
        public async Task<ServiceResponse<string>> AddReferralNote(ReferralNoteDto Note, int UserId)
        {
            try
            {
                var PatientReferralObject = _mapper.Map<PatientReferrer>(Note);
                PatientReferralObject.CreatedBy = UserId;
                PatientReferralObject.CreatedAt = DateTime.UtcNow;
                PatientReferralObject.ActionTaken = "ADDED A CUSTOMER REFERRAL NOTE";
                await _patientReferrerRepository.Insert(PatientReferralObject);
                return new ServiceResponse<string>("Referral note added successfully", InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }
        }

        public async Task<ServiceResponse<string>> DeleteReferralNote(int Id)
        {
            try
            {
                var noteObj = _patientReferrerRepository.GetById(Id);
                if (noteObj == null)
                    return new ServiceResponse<string>("referral note with ID doesnt exist", InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
                await _patientReferrerRepository.DeleteAsync(Id);
                return new ServiceResponse<string>("referral note deleted successfully", InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }
        }

        public async Task<ServiceResponse<IEnumerable<GetPatientReferralDto>>> GetAllReferral()
        {
            try
            {
                var patientObject = _patientRepository.GetAll().Include(x => x.Treatments);
                if (patientObject is null)
                    return new ServiceResponse<IEnumerable<GetPatientReferralDto>>(null, InternalCode.Success, ServiceErrorMessages.Success);
                var ReferrelResponseDto = patientObject.Select(x => new GetPatientReferralDto
                {
                    PatientId = x.Id,
                    ClinicId = x.ClinicId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Diagnosis = x.Treatments.FirstOrDefault().Diagnosis,
                    DateCreated = x.CreatedAt.ToString()
                });
                return new ServiceResponse<IEnumerable<GetPatientReferralDto>>(ReferrelResponseDto, InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<GetPatientReferralDto>>(null, InternalCode.Incompleted, ex.Message);
            }
        }

        public async Task<ServiceResponse<GetPatientReferralDto>> GetAllReferralByPatientId(int patientId)
        {
            try
            {
                var patientObject = _patientRepository.GetAll().Include(x => x.Treatments).FirstOrDefault(x => x.Id == patientId);
                if (patientObject is null)
                    return new ServiceResponse<GetPatientReferralDto>(null, InternalCode.Success, ServiceErrorMessages.Success);
                var ReferrelResponseDto = new GetPatientReferralDto
                {
                    PatientId = patientObject.Id,
                    ClinicId = patientObject.ClinicId,
                    FirstName = patientObject.FirstName,
                    LastName = patientObject.LastName,
                    Diagnosis = patientObject.Treatments.FirstOrDefault()?.Diagnosis,
                    DateCreated = patientObject.CreatedAt.ToString()
                };
                return new ServiceResponse<GetPatientReferralDto>(ReferrelResponseDto, InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetPatientReferralDto>(null, InternalCode.Incompleted, ex.Message);
            }
        }

        public async Task<ServiceResponse<string>> UpdateReferralNote(ReferralNoteDto Note, int UserId)
        {
            try
            {
                var noteObj = _patientReferrerRepository.GetById(Note.PatientId);
                if (noteObj == null)
                    return new ServiceResponse<string>("referral note with ID doesnt exist", InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
                noteObj.Notes = Note.Notes;
                noteObj.UpdatedAt = DateTime.Now;
                noteObj.ModifiedBy = UserId;
                await _patientReferrerRepository.UpdateAsync(noteObj);
                return new ServiceResponse<string>("referral note update successfully", InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }
        }
    }
}
