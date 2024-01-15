using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalRecordsData.Entities
{
    public class Roles
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
