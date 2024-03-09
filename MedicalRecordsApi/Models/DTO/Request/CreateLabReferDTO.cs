using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalRecordsApi.Models.DTO.Request
{
    public class CreateLabReferDto
    {
        public string LabType { get; set; }
        public string LabCentre { get; set; }
        public string LabNote { get; set; }
        public string Diagnosis { get; set; }
        public List<string> LabRequests { get; set; }
    }
}
