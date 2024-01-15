using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalRecordsApi.Utils
{
    public class ApplicationStatus
    {
        public int Active = 1;
        public int NotActive = 2;
        public int Saved = 4;
        public int Pending = 3;
        public int Approved = 7;
        public int Declined = 8;
        public int Delete = 6;
    }
}
