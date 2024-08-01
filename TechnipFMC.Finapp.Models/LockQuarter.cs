using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class LockQuarter : BaseEntity
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Quarter { get; set; }
        public bool Lock { get; set; }
        public string Message { get; set; }
        public string Header { get; set; }
        public string DataEntryInterval { get; set; }
        
    }

    public class QuartersLayOut
    {
        public string qName { get; set; }
        public bool qLock { get; set; }
        public string qHeader { get; set; }
        
    }
}
