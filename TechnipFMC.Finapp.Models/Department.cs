using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class Department : BaseEntity
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public bool Active { get; set; }
    }
}
