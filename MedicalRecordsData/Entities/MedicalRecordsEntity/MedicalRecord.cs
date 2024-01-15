﻿using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsData.Entities.MedicalRecordsEntity
{
	public partial class MedicalRecord : Base
	{
		public MedicalRecordType MedicalRecordType { get; set; } //Allergy etc
		public string Name { get; set; }
		public string Comment { get; set; }

	}

	public enum MedicalRecordType
	{
		Allergy,
		PastIllness,
		SurgicalHistory
	}
}
