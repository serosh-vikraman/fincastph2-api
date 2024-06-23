using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class ActionEntity : BaseEntity
    {
        public int ActionID { get; set; }
        public string ActionCode { get; set; }
        public string ActionName { get; set; }       
    }
}
