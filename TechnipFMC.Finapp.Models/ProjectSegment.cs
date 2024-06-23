using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ProjectSegment : BaseEntity
    {
        public int ProjectSegmentID { get; set; }
        public string ProjectSegmentCode { get; set; }
        public string ProjectSegmentName { get; set; }
    }
}
