
using MedicalRecordsRepository.DTO.PatientDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecordsRepository.DTO.MedicalDto
{
    public class ImmunizationDto
    {
        [Required(ErrorMessage= "the Vaccine is required")]
        public string Vaccine { get; set; }
        [Required(ErrorMessage= "the VaccineBrand is required")]
        public string VaccineBrand { get; set; }
        [Required(ErrorMessage= "the BatchId is required")]
        public string BatchId { get; set; }
        [Required(ErrorMessage= "the Quantity is required")]
        public double Quantity { get; set; }
        [Required(ErrorMessage= "the Age is required")]
        public int? Age { get; set; }
        [Required(ErrorMessage= "the Weight is required")]
        public double? Weight { get; set; }
        [Required(ErrorMessage= "the Temperature is required")]
        public double? Temperature { get; set; }
        [Required(ErrorMessage= "the DateGiven is required")]
        [DataType(DataType.DateTime)]
        [DateNotGreaterThanNow(ErrorMessage = "Date should not be greater than current date")]
        public DateTime DateGiven { get; set; }
        [Required(ErrorMessage= "the Notes is required")]
        public string Notes { get; set; }
        //Navigation Properties
        [Required(ErrorMessage= "the PatientId is required")]
        public int? PatientId { get; set; }
        public string DocName { get; set; }
        public string DocPath { get; set; }
    }
    public class ResponseImmunizationDto
    {
        public string Vaccine { get; set; }
        public string VaccineBrand { get; set; }
        public string BatchId { get; set; }
        public double Quantity { get; set; }
        public int? Age { get; set; }
        public double Weight { get; set; }
        public double Temperature { get; set; }
        public DateTime DateGiven { get; set; }
        public string Notes { get; set; }
        public int? PatientId { get; set; }
        public int ImmunizationId { get; set; }
        public List<ListOfDocument> ImmunizationDocuments { get; set; }
    }
    public class ListOfDocument
    {
        public string DocName { get; set; }
        public string DocPath { get; set; }
    }
}
