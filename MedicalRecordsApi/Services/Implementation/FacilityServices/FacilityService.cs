﻿using AutoMapper;
using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services.Abstract.FacilityInterfaces;
using MedicalRecordsData.DatabaseContext;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.DTO.FacilityDto;
using MedicalRecordsRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Implementation.FacilityServices
{
    public class FacilityService : IFacilityService
    {
        #region config
        private readonly IMapper _mapper;
        private readonly MedicalRecordDbContext _dbContext;
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IGenericRepository<Facility> _facilityRepository;
        private readonly IGenericRepository<BedAssignment> _bedAssignmentRepository;
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<User> _userRepository;

        public FacilityService(IMapper mapper, MedicalRecordDbContext dbContext, 
            IGenericRepository<Patient> patientRepository, IGenericRepository<Facility> facilityRepository, 
            IGenericRepository<BedAssignment> bedAssignmentRepository, IConfiguration configuration, 
            IGenericRepository<User> userRepository)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _patientRepository = patientRepository;
            _facilityRepository = facilityRepository;
            _bedAssignmentRepository = bedAssignmentRepository;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse<string>> AssignBed(AssignBedRequestDto bedSpaceDto, int userId)
        {
            try
            {
                var FacilityResponse = _facilityRepository.GetById(bedSpaceDto.PatientId);
                if (FacilityResponse is null || FacilityResponse.IsOccupied)
                {
                    return new ServiceResponse<string>("unable to complete since the bedSpace is not available", InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
                }
                var bedSpaceObject = _mapper.Map<BedAssignment>(bedSpaceDto);
                bedSpaceObject.CreatedAt = DateTime.Now;
                bedSpaceObject.CreatedBy = userId;
                bedSpaceObject.ActionTaken = "Assigned BedSpace";
                await _bedAssignmentRepository.Insert(bedSpaceObject);
                FacilityResponse.IsOccupied = true;
                await _facilityRepository.UpdateAsync(FacilityResponse);
                return new ServiceResponse<string>("bedSpace successfully Assigned", InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }
            
        }

        public async Task<ServiceResponse<string>> FreeBedSpace(int patientBedSpace, int userId)
        {
            try
            {
                var FacilityResponse = _facilityRepository.GetById(patientBedSpace);
                if (FacilityResponse is null || !FacilityResponse.IsOccupied)
                {
                    return new ServiceResponse<string>("unable to complete since the bedSpace is not available or is already free", InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
                }
                var bedSpaceObject = await _bedAssignmentRepository.GetByIdAsync(patientBedSpace);
                bedSpaceObject.CreatedAt = DateTime.Now;
                bedSpaceObject.CreatedBy = userId;
                bedSpaceObject.ActionTaken = "Assigned BedSpace";
                await _bedAssignmentRepository.DeleteAsync(patientBedSpace);
                FacilityResponse.IsOccupied = false;
                await _facilityRepository.UpdateAsync(FacilityResponse);
                return new ServiceResponse<string>("bedSpace successfully removed", InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }

        }
        #endregion

        public async Task<ServiceResponse<IEnumerable<ReadBedDetailsDTO>>> GetBedsAssignedToDoctor(int userId)
        {
            if (userId <= 0)
            {
                return new ServiceResponse<IEnumerable<ReadBedDetailsDTO>>(Enumerable.Empty<ReadBedDetailsDTO>(), InternalCode.EntityIsNull, ServiceErrorMessages.ParameterEmptyOrNull);
            }


            //Get Facility Details
            var beds = await _facilityRepository.Query()
                                                .AsNoTracking()
                                                .Where(x => x.FacilityType == FacilityType.Bed)
                                                .ToListAsync();
            if (!beds.Any())
            {
                return new ServiceResponse<IEnumerable<ReadBedDetailsDTO>>(Enumerable.Empty<ReadBedDetailsDTO>(), InternalCode.Success);

            }

            //Get Patients
            var patients = await _patientRepository.Query()
                                                  .AsNoTracking()
                                                  .Where(x => x.DoctorId == userId).ToListAsync();

            //Get Bed Assignment
            var bedAssignments = await _bedAssignmentRepository.Query()
                                                               .AsNoTracking() 
                                                               .Where(x => x.IsDeleted == false)
                                                               .ToListAsync();

            List<ReadBedDetailsDTO> bedDetailsList = new List<ReadBedDetailsDTO>();

            foreach ( var bed in beds )
            {
                ReadBedDetailsDTO bedDetailsDTO = new ReadBedDetailsDTO();
                bedDetailsDTO.BedName = bed.Name;
                bedDetailsDTO.IsOccupied = bed.IsOccupied;

                if (bed.IsOccupied)
                {
                    string patientName = patients.Where(x => x.Id == bedAssignments.FirstOrDefault(x => x.FacilityId == bed.Id).PatientId)
                                                                                .Select(s => $"{s.FirstName} {s.LastName}")
                                                                                .FirstOrDefault();

                    bedDetailsDTO.PatientName = patientName ?? null;
                    bedDetailsDTO.PatientId = bedAssignments.FirstOrDefault(x => x.FacilityId == bed.Id).PatientId;
                }

                bedDetailsList.Add(bedDetailsDTO);
            }

            return new ServiceResponse<IEnumerable<ReadBedDetailsDTO>>(bedDetailsList, InternalCode.Success);
        }

        public async Task<ServiceResponse<IEnumerable<ReadBedDetailsDTO>>> GetBedStatus()
        {
            //Get Facility Details
            var beds = await _facilityRepository.Query()
                                                .AsNoTracking()
                                                .Where(x => x.FacilityType == FacilityType.Bed)
                                                .ToListAsync();
            if (!beds.Any())
            {
                return new ServiceResponse<IEnumerable<ReadBedDetailsDTO>>(Enumerable.Empty<ReadBedDetailsDTO>(), InternalCode.Success);

            }

            List<ReadBedDetailsDTO> bedDetailsList = new List<ReadBedDetailsDTO>();

            foreach (var bed in beds)
            {
                ReadBedDetailsDTO bedDetailsDTO = new ReadBedDetailsDTO();
                bedDetailsDTO.BedName = bed.Name;
                bedDetailsDTO.IsOccupied = bed.IsOccupied;

                bedDetailsList.Add(bedDetailsDTO);
            }

            return new ServiceResponse<IEnumerable<ReadBedDetailsDTO>>(bedDetailsList, InternalCode.Success);
        }
    }
}