using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using back_end_structure.Models;

namespace back_end_structure.Data
{
    public class MedicalRecordsContext : DbContext
    {
        public MedicalRecordsContext (DbContextOptions<MedicalRecordsContext> options)
            : base(options)
        {
        }

        public DbSet<back_end_structure.Models.Employees> Employees { get; set; }
        public DbSet<back_end_structure.Models.Sample> Sample { get; set; }
        public DbSet<back_end_structure.Models.UserRoles> UserRoles { get; set; }
        public DbSet<back_end_structure.Models.Roles> Roles { get; set; }
        public DbSet<back_end_structure.Models.Resources> Resources { get; set; }
    }
}
