using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Utils
{
	public class RequestParameters
	{
		const int maxPageSize = 100;
		public int PageNumber { get; set; } = 1;
		private int _pageSize = 10;
		public string Status { get; set; }

		public string QueryFilter { get; set; }
		public int PageSize
		{
			get
			{
				return _pageSize;
			}
			set
			{
				_pageSize = (value > maxPageSize) ? maxPageSize : value;
			}
		}
	}

	public class PageResponse
	{
		public int TotalCount { get; set; }
		public int PageSize { get; set; }
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public bool HasNext { get; set; }
		public bool HasPrevious { get; set; }
	}
}
