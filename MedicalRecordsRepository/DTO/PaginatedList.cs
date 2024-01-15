using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsRepository.DTO
{
	public class PaginatedList<T> where T : class
	{
		public IEnumerable<T> Data { get; set; } = new List<T>();
		public int PageCount { get; set; }
		public int PageIndex { get; set; } = 1;
		public int PageSize { get; set; }
	}
}
