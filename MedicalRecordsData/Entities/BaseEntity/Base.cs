using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MedicalRecordsData.Entities.BaseEntity
{
	public partial class Base
	{
		[Key]
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public bool IsDeleted { get; set; }
	}
}
