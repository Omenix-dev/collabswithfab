using System;

namespace MedicalRecordsApi.Models.DTO.Responses
{
    public class ReadBedDetailsDto
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public string BedName { get; set; }
        public string IsOccupied { get; set; }
        public string PatientName { get; set; }
        public int PatientId { get; set; }
    }
}
