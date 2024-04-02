namespace MedicalRecordsApi.Models.DTO.Responses
{
    public class GetAllNurseDto
    {
        public int NurseEmployeeId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public int StaffId { get; set; }
        public int EmployeeId { get; set; }
        public int? ClinicId { get; set; }
    }

    public class GetAllDoctorDto
    {
        public int DoctorEmployeeId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public int StaffId { get; set; }
        public int EmployeeId { get; set; }
        public int? ClinicId { get; set; }
    }
}
