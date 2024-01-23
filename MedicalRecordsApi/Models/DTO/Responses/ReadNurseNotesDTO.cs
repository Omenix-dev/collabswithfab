using System.Collections.Generic;

namespace MedicalRecordsApi.Models.DTO.Responses
{
	public class ReadNurseNotesDTO
	{
        public ReadVisitHistoryDTO Visit { get; set; }
        public List<NurseNotesDTO> Notes { get; set; }
    }

	public class NurseNotesDTO
	{
		public string Note { get; set; }
	}
}
