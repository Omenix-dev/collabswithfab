
using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalRecordsRepository.DTO.PatientDto
{
    public class PatientsVisitsDto
    {
        [Required(ErrorMessage = "The DateOfVisit is required")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [DateNotGreaterThanNow(ErrorMessage = "Date should not be greater than current date")]
        public DateTime DateOfVisit { get; set; }
        [Required(ErrorMessage = "The Temperature is required")]
        public double Temperature { get; set; }
        [Required(ErrorMessage = "The BloodPressure is required")]
        public string BloodPressure { get; set; }
        [Required(ErrorMessage = "The HeartPulse is required")]
        public int? HeartPulse { get; set; }
        [Required(ErrorMessage = "The Respiratory is required")]
        public string Respiratory { get; set; }
        [Required(ErrorMessage = "The Height is required")]
        public int? Height { get; set; }
        [Required(ErrorMessage = "The Weight is required")]
        public int? Weight { get; set; }
        [Required(ErrorMessage = "The DoctorId is required")]
        public int? DoctorEmployeeId { get; set; }
        [Required(ErrorMessage = "The NurseId is required")]
        public int? NurseEmployeeId { get;set; }
        [Required(ErrorMessage = "The Notes is required")]
        public string Notes { get; set; }
        [Required(ErrorMessage = "The PatientId is required")]
        public int? PatientId { get; set; }
    }
    public class ResponsePatientsVisitsDto
    {
        public DateTime DateOfVisit { get; set; }
        public double Temperature { get; set; }
        public string BloodPressure { get; set; }
        public int? HeartPulse { get; set; }
        public string Respiratory { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public int? DoctorId { get; set; }
        public int? NurseId { get; set; }
        public string Notes { get; set; }
        public int? PatientId { get; set; }
        public int VisitId { get; set; }
    }
    public class DateNotGreaterThanNowAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true; // Let the RequiredAttribute handle this case

            DateTime dateValue = (DateTime)value;
            return dateValue <= DateTime.Now;
        }
    }
}
