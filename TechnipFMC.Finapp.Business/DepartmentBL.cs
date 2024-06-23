using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnipFMC.Finapp.Data;
using TechnipFMC.Finapp.Data.Interfaces;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public class DepartmentBL : IDepartmentBL
    {
        public DepartmentBL(IDepartmentRepository DepartmentRepo)
        {
            //_DepartmentRepo = DepartmentRepo;
        }
        public DepartmentBL()
        { }
        public bool Delete(int Id, int Deletedby)
        {
            DepartmentRepository _DepartmentRepo = new DepartmentRepository();
            return _DepartmentRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<Department> GetAll()
        {
            DepartmentRepository _DepartmentRepo = new DepartmentRepository();
            return _DepartmentRepo.GetAll();
        }

        public Department GetById(int id )
        {
            DepartmentRepository _DepartmentRepo = new DepartmentRepository();
            return _DepartmentRepo.GetById(id);
        }

        public Department Save(Department Department)
        {
            DepartmentRepository _DepartmentRepo = new DepartmentRepository();
            return _DepartmentRepo.Save(Department);
        }

        public Department Update(Department Department)
        {
            throw new NotImplementedException();
        }
    }
}
