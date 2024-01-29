namespace MedicalRecordsApi.Models.DTO.Responses
{
    public class ReadBedDetailsDTO
    {
        public string BedName { get; set; }
        public bool IsOccupied { get; set; }
        public string PatientName { get; set; }
        public int PatientId { get; set; }
    }
}
