using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public interface IDepartmentBL
    {
        IEnumerable<Department> GetAll();
        Department Save(Department department);
        Department GetById(int Id);

        Department Update(Department department);

        bool Delete(int Id, int Deletedby);
    }
}
