using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IManagementCategoryBL
    {
        IEnumerable<ManagementCategory> GetAll();
        ManagementCategory Save(ManagementCategory managementcategory);
        ManagementCategory GetById(int Id);

        ManagementCategory Update(ManagementCategory managementcategory);

        bool Delete(int Id, string Deletedby);
    }
}
