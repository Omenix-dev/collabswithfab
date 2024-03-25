﻿using AutoMapper;
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
        public ReferralServices(IGenericRepository<Patient> patientRepository, 
            IGenericRepository<PatientReferrer> patientReferrerRepository, IGenericRepository<Clinic> clinicRepository)
        {
            _patientRepository = patientRepository;
            _patientReferrerRepository = patientReferrerRepository;
            _clinicRepository = clinicRepository;
        }
        public async Task<ServiceResponse<string>> AddReferral(ReferralDto Note, int UserId)
        {
            try
            {
                var patientObject = _patientRepository.GetById(Note.PatientId);
                if (patientObject == null)
                    return new ServiceResponse<string>("The Patient doesnt Exist", InternalCode.Success, "The Patient doesnt Exist");
                var clinicObject = _clinicRepository.GetById(Note.ClinicId);
                if (clinicObject == null)
                    return new ServiceResponse<string>("The Clinic doenst Exist", InternalCode.Success, "The Clinic doenst Exist");
                var ReferralclinicObject = _clinicRepository.GetById(Note.ReferredClinicId);
                if (clinicObject == null)
                    return new ServiceResponse<string>("The Referral Clinic doenst Exist on the System", InternalCode.Success, "The Referral Clinic doenst Exist on the System");

                var PatientReferralObject = new PatientReferrer();
                PatientReferralObject.AcceptanceStatus = AcceptanceStatus.Pending;
                PatientReferralObject.ReferredClinicId = Note.ReferredClinicId;
                PatientReferralObject.TreatmentId = Note.TreatmentId;
                PatientReferralObject.PatientId = Note.PatientId;
                PatientReferralObject.ClinicId = Note.ClinicId;
                PatientReferralObject.CreatedBy = UserId;
                PatientReferralObject.CreatedAt = DateTime.UtcNow;
                PatientReferralObject.ActionTaken = "ADDED A CUSTOMER TO REFERRAL Table";
                await _patientReferrerRepository.Insert(PatientReferralObject);
                return new ServiceResponse<string>("Referral note added successfully", InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ServiceErrorMessages.Incompleted);
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
                var ReferredPatient = _patientReferrerRepository.GetAll().Where(x => x.ClinicId == ClinicId &&
                                     x.AcceptanceStatus == AcceptanceStatus.Pending).Include(x => x.Treatment)
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

        public ServiceResponse<GetPatientReferralDto> GetAllReferralByPatientId(int ClinicId, int patientId)
        {
            try
            {
                var patientExist = _patientRepository.GetById(patientId);
                if (patientExist == null)
                {
                    return new ServiceResponse<GetPatientReferralDto>(null, InternalCode.Unprocessable, "Patient doesnt Exist");
                }
                var clinicExist = _clinicRepository.GetById(ClinicId);
                if (clinicExist == null)
                {
                    return new ServiceResponse<GetPatientReferralDto>(null, InternalCode.Unprocessable, "Invalid Clinic");
                }
                var ReferredPatient = _patientReferrerRepository.GetAll().Where(x => x.ClinicId == ClinicId &&
                                    x.PatientId == patientId && x.AcceptanceStatus == AcceptanceStatus.Pending)
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
                return new ServiceResponse<GetPatientReferralDto>(ReferredPatient, InternalCode.Success, ServiceErrorMessages.Success);
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
                var noteObj = _patientReferrerRepository.GetAll().Include(x => x.Treatment).Include(x => x.Patient).Where(x => x.Id == Note.ReferredId).First();
                if (noteObj == null)
                    return new ServiceResponse<string>($"Referral with Id {Note.ReferredId} doesnt exist", InternalCode.Incompleted, $"Referral with Id {Note.ReferredId} doesnt exist");
                noteObj.Notes = Note.Notes;
                noteObj.UpdatedAt = DateTime.Now;
                noteObj.ModifiedBy = UserId;
                noteObj.AcceptanceStatus = Note.AcceptanceStatus;
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
                return new ServiceResponse<string>("Referral note update successfully", InternalCode.Success, ServiceErrorMessages.Success);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>(ex.Message, InternalCode.Incompleted, ex.Message);
            }
        }
    }
}
