using System;
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

namespace MedicalRecordsData.DatabaseContext
{
	public partial class MedicalRecordDbContext : DbContext
	{
		public MedicalRecordDbContext(DbContextOptions<MedicalRecordDbContext> options)
			: base(options)
		{
		}

		//Base DbSet by Edward
		public DbSet<Employees> Employees { get; set; }
		public DbSet<Sample> Sample { get; set; }
		public DbSet<UserRoles> UserRoles { get; set; }
		public DbSet<Roles> Roles { get; set; }
		public DbSet<Resources> Resources { get; set; }


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
