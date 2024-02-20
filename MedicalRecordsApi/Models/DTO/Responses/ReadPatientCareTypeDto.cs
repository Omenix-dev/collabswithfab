using System.Collections.Generic;

namespace MedicalRecordsApi.Models.DTO.Responses
{
    public class ReadPatientCareTypeDto
    {
        public double InPatientPercentage { get; set; }
        public double OutPatientPercentage { get; set; }
        public List<DailyAverageCount> DailyAverageCount { get; set;}
    }

    public class DailyAverageCount
    {
        public string Date { get; set; }
        public long InPatientCount { get; set; }
        public long OutPatientCount { get; set; }
    }
}
