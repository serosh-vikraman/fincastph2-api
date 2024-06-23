using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Finapp.Models
{
    public class LockYear : BaseEntity
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public bool Lock { get; set; }

        public string Message { get; set; }

    }
}
