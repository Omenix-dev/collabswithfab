using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MedicalRecordsData.Entities.BaseEntity;
using Newtonsoft.Json;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class LabRequest : Base
    {
        public string LabType { get; set; }
        public string LabCentre { get; set; }
        private string LabRequestsList { get; set; }
        public string LabNote { get; set; } // Rich text format
		public string Diagnosis { get; set; }

		[NotMapped]
		public List<string> LabRequests
		{
			get => JsonConvert.DeserializeObject<List<string>>(LabRequestsList);
			set => LabRequestsList = JsonConvert.SerializeObject(value);
		}
		
		
		//Navigation Properties
		public int VisitId { get; set; }
		public virtual Visit Visit { get; set; }
	}
}
