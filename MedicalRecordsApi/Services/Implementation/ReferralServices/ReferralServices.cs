using AutoMapper;
using MedicalRecordsApi.Constants;
using MedicalRecordsApi.Models.DTO.Responses;
using MedicalRecordsApi.Services.Abstract.ReferralInterfaces;
using MedicalRecordsApi.Services.Common;
using MedicalRecordsData.DatabaseContext;
using MedicalRecordsData.Entities.AuthEntity;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsData.Enum;
using MedicalRecordsRepository.DTO;
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
        private readonly IGenericRepository<Patient> _patientRepository;
        private readonly IGenericRepository<PatientReferrer> _patientReferrerRepository;
        private readonly IGenericRepository<Clinic> _clinicRepository;
        private readonly IGenericRepository<Treatment> _treatmentRepository;
        private readonly IMapper _mapper;
        public ReferralServices(IGenericRepository<Patient> patientRepository,
            IGenericRepository<PatientReferrer> patientReferrerRepository,
            IGenericRepository<Clinic> clinicRepository,
            IGenericRepository<Treatment> treatmentRepository,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _patientReferrerRepository = patientReferrerRepository;
            _clinicRepository = clinicRepository;
            _treatmentRepository = treatmentRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<object>> AddReferral(ReferralDto Note, int UserId)
        {
            try
            {
                var patientObject = _patientRepository.GetById(Note.PatientId);
                if (patientObject == null)
                    return new ServiceResponse<object>("The Patient doesnt Exist", InternalCode.EntityNotFound, "The Patient doesnt Exist");
                var clinicObject = _clinicRepository.GetById(Note.ClinicId);
                if (clinicObject == null)
                    return new ServiceResponse<object>("The Clinic doenst Exist", InternalCode.EntityNotFound, "The Clinic doenst Exist");
                var ReferralclinicObject = _clinicRepository.GetById(Note.ReferredClinicId);
                if (clinicObject == null)
                    return new ServiceResponse<object>("The Referral Clinic doenst Exist on the System", InternalCode.EntityNotFound, "The Referral Clinic doenst Exist on the System");
                var treatmentObject = _treatmentRepository.GetById(Note.TreatmentId);
                if (treatmentObject == null)
                    return new ServiceResponse<object>("This treatment case doesnt exist", InternalCode.EntityNotFound, "This treatment case doesn't exist");
                
                var PatientReferralObject =_mapper.Map<PatientReferrer>(Note);
                PatientReferralObject.AcceptanceStatus = AcceptanceStatus.Pending;
                PatientReferralObject.ReferrerNote = Note.ReferralNotes;
                PatientReferralObject.CreatedBy = UserId;
                PatientReferralObject.CreatedAt = DateTime.UtcNow;
                PatientReferralObject.ActionTaken = "ADDED A CUSTOMER TO REFERRAL TABLE";
                await _patientReferrerRepository.Insert(PatientReferralObject);
                return new ServiceResponse<object>(new { Message = "Referral note added successfully" }, InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<object>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
            }
        }

        public async Task<ServiceResponse<object>> RemoveReferredPatient(int Id)
        {
            try
            {
                var noteObj = _patientReferrerRepository.GetById(Id);
                if (noteObj == null)
                    return new ServiceResponse<object>($"There is no such referral with Id {Id}", InternalCode.Unprocessable, $"There is no such referral with Id {Id}");
                if (noteObj.AcceptanceStatus == AcceptanceStatus.Accepted)
                    return new ServiceResponse<object>($"Cant Delete ReferralId {Id}, It has already been accepted", InternalCode.Unprocessable, $"Cant Delete ReferralId {Id}, It has already been accepted");
                await _patientReferrerRepository.DeleteAsync(Id);
                return new ServiceResponse<object>(new { Message = "The Patient has been removed from referral table", ReferralId = Id }, InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<object>(ex.Message, InternalCode.Incompleted, ex.Message);
            }
        }

        public ServiceResponse<PaginatedList<GetPatientReferralDto>> GetAllReferral(int ClinicId, int pageIndex, int pageSize)
        {
            try
            {
                var clinicExist = _clinicRepository.GetById(ClinicId);
                if (clinicExist == null)
                {
                    return new ServiceResponse<PaginatedList<GetPatientReferralDto>>(null, InternalCode.Unprocessable, "Invalid Clinic");
                }
                var ReferredPatient = _patientReferrerRepository.GetAll().Where(x => x.ClinicId == ClinicId).Include(x => x.Treatment)
                                     .Include(x => x.Clinic).Include(x => x.Patient);
                if (ReferredPatient is null)
                    return new ServiceResponse<PaginatedList<GetPatientReferralDto>>(null, InternalCode.Success, ServiceErrorMessages.Success);
                var ReferrelResponseDto = ReferredPatient.Select(x => new GetPatientReferralDto
                {
                    ReferralId = x.Id,
                    PatientId = x.PatientId,
                    HospitalName = x.Clinic.Name,
                    FirstName = x.Patient.FirstName,
                    LastName = x.Patient.LastName,
                    Diagnosis = x.Treatment.Diagnosis,
                    DateCreated = x.CreatedAt.ToString(),
                    AcceptanceStatus = x.AcceptanceStatus.ToString(),
                });
                var valObject = new GenericService<GetPatientReferralDto>().SortPaginateByText(pageIndex, pageSize, ReferrelResponseDto, x => x.ReferralId.ToString(), Order.Asc);
                return new ServiceResponse<PaginatedList<GetPatientReferralDto>>(valObject, InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<PaginatedList<GetPatientReferralDto>>(null, InternalCode.Incompleted, ex.Message);
            }
        }

        public ServiceResponse<GetPatientReferralDto> GetAllReferralByReferralId(int ReferralId)
        {
            try
            {

                var ReferredPatient = _patientReferrerRepository.GetAll()
                                      .Where(x => x.Id == ReferralId)
                                    .Include(x => x.Treatment).Include(x => x.Clinic).Include(x => x.Patient).
                                    Select(x => new GetPatientReferralDto
                                    {
                                        ReferralId = x.Id,
                                        PatientId = x.PatientId,
                                        HospitalName = x.Clinic.Name,
                                        FirstName = x.Patient.FirstName,
                                        LastName = x.Patient.LastName,
                                        Diagnosis = x.Treatment.Diagnosis,
                                        DateCreated = x.CreatedAt.ToString(),
                                        AcceptanceStatus = x.AcceptanceStatus.ToString(),
                                    }).FirstOrDefault();
                if(ReferredPatient is null)
                    return new ServiceResponse<GetPatientReferralDto>(null, InternalCode.EntityNotFound, "There is no patient referred with the Id");

                return new ServiceResponse<GetPatientReferralDto>(ReferredPatient, InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetPatientReferralDto>(null, InternalCode.Incompleted, ex.Message);
            }
        }

        public async Task<ServiceResponse<object>> UpdateReferralNote(ReferralNoteDto Note, int UserId)
        {
            try
            {
                var noteObj = _patientReferrerRepository.GetAll().Include(x => x.Treatment).Include(x => x.Patient).Where(x => x.Id == Note.ReferredId).First();
                if (noteObj == null)
                    return new ServiceResponse<object>($"Referral with Id {Note.ReferredId} doesnt exist", InternalCode.Incompleted, $"Referral with Id {Note.ReferredId} doesnt exist");
                noteObj.Notes = Note.Notes;
                noteObj.UpdatedAt = DateTime.Now;
                noteObj.ModifiedBy = UserId;
                noteObj.AcceptanceStatus = Note.AcceptanceStatus;
                noteObj.Notes = Note.Notes;
                await _patientReferrerRepository.UpdateAsync(noteObj);
                if(noteObj.AcceptanceStatus == AcceptanceStatus.Accepted)
                {
                    var patientData = noteObj.Patient;
                    var Exist = _patientRepository.GetAll().Where(x => x.Email == patientData.Email && x.ClinicId == noteObj.ReferredClinicId).FirstOrDefault();
                    if(Exist == null)
                    {
                        patientData.ClinicId = noteObj.ReferredClinicId;
                        patientData.CreatedAt = DateTime.Now;
                        patientData.CreatedBy = UserId;
                        patientData.IsReferred = true;
                        patientData.Id = 0;
                        patientData.NurseId = null;
                        patientData.DoctorId = null;
                        patientData.Treatments.Add(noteObj.Treatment);
                        await _patientRepository.CreateAsync(patientData);
                    }
                    else
                    {
                        // only added the treatments for situation where the user already exist
                        Exist.Treatments.Add(noteObj.Treatment);
                        await _patientRepository.Update(Exist);
                    }

                    // Transfer the patient data to the present clinic database
                }
                return new ServiceResponse<object>(new { Message = "Referral note updated successfully" }, InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<object>(ex.Message, InternalCode.Incompleted, ex.Message);
            }
        }
    }
}
