using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
     public class ProjectEntity : BaseEntity
    {
        public int ProjectEntityID { get; set; }
        public string ProjectEntityCode { get; set; }
        public string ProjectEntityName { get; set; }
    }
}
