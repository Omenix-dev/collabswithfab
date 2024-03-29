using AutoMapper;
using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Models.DTO.Request;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services.Abstract.HMOInterface;
using MedicalRecordsApi.Services.Common.Interfaces;
using MedicalRecordsData.DatabaseContext;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsRepository.Interfaces;
using System;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Services.Implementation.HMOServices
{

    public class HMOService : IHMOService
    {
        private readonly IMapper _mapper;
        private readonly MedicalRecordDbContext _dbContext;
        private readonly IGenericService<AssignedPatientsDto> _genericAssignedPatientService;
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IGenericRepository<PatientHmo> _patientHmoRepository;
        public HMOService(IMapper mapper, MedicalRecordDbContext dbContext,
                          IGenericService<AssignedPatientsDto> genericAssignedPatientService,
                          IGenericRepository<Patient> patientRepository)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _genericAssignedPatientService = genericAssignedPatientService;
            _patientRepository = patientRepository;
        }
        public async Task<ServiceResponse<object>> AddPatientHMOAsync(PatientHMODto patientHMODto, int UserId)
        {
            try
            {
                //HMOProvider verify HMO provider services

                //HMOPackageId verify HMO package Id

                //PatientHMOId verify HMO patient Id
                var patientObj = _patientRepository.GetById(patientHMODto.PatientId);
                if (patientObj == null)
                    return new ServiceResponse<object>(String.Empty, InternalCode.EntityIsNull, "The Patient doesnt Exist");
                var PatientHMO = _mapper.Map<PatientHmo>(patientObj);
                PatientHMO.CreatedAt = DateTime.UtcNow;
                PatientHMO.CreatedBy = UserId;
                await _patientHmoRepository.Insert(PatientHMO);
                return new ServiceResponse<object>(new { Message = "Successfully added the patient HMO" }, InternalCode.Success);
            }
            catch (System.Exception ex)
            {
                return new ServiceResponse<object>(String.Empty, InternalCode.Incompleted, ex.Message);
            }
        }
        public async Task<ServiceResponse<object>> UpdatePatientHMOAsync(UpdatePatientHMODto updatePatientHMODto, int UserId)
        {
            try
            {
                //HMOProvider verify HMO provider services

                //HMOPackageId verify HMO package Id

                //PatientHMOId verify HMO patient Id
                var PatientHMO = _patientHmoRepository.GetById(updatePatientHMODto.Id);
                if (PatientHMO == null)
                    return new ServiceResponse<object>(String.Empty, InternalCode.EntityIsNull, "The Patient doesnt Exist");
                PatientHMO.UpdatedAt = DateTime.UtcNow;
                PatientHMO.ModifiedBy = UserId;
                PatientHMO.HMOProviderId = updatePatientHMODto.HMOProviderId;
                PatientHMO.HMOPackageId = updatePatientHMODto.HMOPackageId;
                PatientHMO.PatientHMOId = updatePatientHMODto.PatientHMOId;
                PatientHMO.MembershipValidity = updatePatientHMODto.MembershipValidity;
                PatientHMO.Notes = updatePatientHMODto.Notes;
                PatientHMO.PatientHMOCardDocumentUrl = updatePatientHMODto.PatientHMOCardDocumentUrl;
                await _patientHmoRepository.Insert(PatientHMO);
                return new ServiceResponse<object>(new { Message = "Successfully added the patient HMO" }, InternalCode.Success);
            }
            catch (System.Exception ex)
            {
                return new ServiceResponse<object>(String.Empty, InternalCode.Incompleted, ex.Message);
            }
        }
    }
}
