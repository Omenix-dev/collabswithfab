﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MedicalRecordsData.Entities;
using MedicalRecordsData.Entities.MedicalRecordsEntity;
using MedicalRecordsData.Entities.AuthEntity;

namespace MedicalRecordsData.DatabaseContext
{
	public partial class MedicalRecordDbContext : DbContext
	{
		public MedicalRecordDbContext(DbContextOptions<MedicalRecordDbContext> options)
			: base(options)
		{
		}

		//Base DbSet by Edward
		//public virtual DbSet<Role> Roles { get; set; }
		//public virtual DbSet<User> Users { get; set; }
		//public virtual DbSet<Resources> Resources { get; set; }
		//public virtual DbSet<Employee> Employees { get; set; }
		//public virtual DbSet<EmployeePrivilegeAccess> EmployeePrivilegeAccesses { get; set; }


		//Patient DbSets
		public virtual DbSet<Patient> Patients { get; set; }
		public virtual DbSet<PatientReferrer> PatientReferrers { get; set; }
		public virtual DbSet<Treatment> Treatments { get; set; }
		public virtual DbSet<Visit> Visits { get; set; }
		public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }
		public virtual DbSet<Medication> Medications { get; set; }
		public virtual DbSet<Immunization> Immunizations { get; set; }
		public virtual DbSet<ImmunizationDocument> ImmunizationDocuments { get; set; }
		public virtual DbSet<EmergencyContact> EmergencyContacts { get; set; }
		public virtual DbSet<Contact> Contacts { get; set; }
		public virtual DbSet<PatientLabReport> PatientLabReports { get; set; }
		public virtual DbSet<PatientLabDocument> PatientLabDocuments { get; set; }
		public virtual DbSet<Facility> Facilities { get; set; }
		public virtual DbSet<BedAssignment> BedAssignments { get; set; }
		public virtual DbSet<LabRequest> LabRequests { get; set; }
		public virtual DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }
		public virtual DbSet<PatientAssignmentHistory> PatientAssignmentHistories { get; set; }



		//Regenerate Models and DBContext using CodeFirst From Database
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			////Ensure all dates are saved as UTC and read as UTC:
			////https://github.com/dotnet/efcore/issues/4711#issuecomment-481215673

			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				foreach (var property in entityType.GetProperties())
				{
					if (property.ClrType == typeof(DateTime))
					{
						modelBuilder.Entity(entityType.ClrType)
						 .Property<DateTime>(property.Name)
						 .HasConversion(
						  v => v.ToUniversalTime(),
						  v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
					}
					else if (property.ClrType == typeof(DateTime?))
					{
						modelBuilder.Entity(entityType.ClrType)
						 .Property<DateTime?>(property.Name)
						 .HasConversion(
						  v => v.HasValue ? v.Value.ToUniversalTime() : v,
						  v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);
					}
				}
			}


			modelBuilder.Entity<Visit>()
				.HasOne(v => v.Treatment)
				.WithOne(t => t.Visit)
				.HasForeignKey<Treatment>(t => t.VisitId)
				.OnDelete(DeleteBehavior.NoAction);
		}

		//TODO: Check performance
		//To set the DateUpdated automatically.
		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker
				.Entries()
				.Where(e => e.State == EntityState.Modified);

			foreach (var entityEntry in entries)
			{
				var dateUpdatedProp = entityEntry.Metadata.FindProperty("DateUpdated");
				if (dateUpdatedProp != null)
				{
					entityEntry.Property("DateUpdated").CurrentValue = DateTime.UtcNow;
				}
			}

			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
	}
}
