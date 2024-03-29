﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalRecordsRepository
{
	internal static class RepositoryConstants
	{
		public const string LoggingStarted = "Started logging";
		public const string CreateNullError = "Attempt to insert empty entity. Type of Entity : {0}";
		public const string DeleteNullError = "Could not find entity for deleting. type of Entity : {0}";
		public const string BulkDeleteNullError = "Attempt to Delete empty list of entities. Type of Entity : {0}";
		public const string BulkCreateNullError = "Attempt to insert empty list of entities. Type of Entity : {0}";
		public const string EmptySaveInfo = "No changes was written to underlying database.";
		public const string UpdateException = "Update Exception";
		public const string UpdateConcurrencyException = "Update Concurrency Exception";
		public const string SaveChangesException = "Generic Error in Generic Repo Update method";
	}
}
