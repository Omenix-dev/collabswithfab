using System;
using System.Collections.Generic;

namespace MedicalRecordsApi.Models.DTO.Responses
{
	public class ReadNurseNotesDto
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public ReadVisitHistoryDto Visit { get; set; }
        public List<NurseNotesDto> Notes { get; set; }
    }

	public class NurseNotesDto
	{
		public string Note { get; set; }
	}
}
