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
    public class ManagementCategoryBL : IManagementCategoryBL
    {
        public ManagementCategoryBL(IManagementCategoryRepository managementcategoryRepo)
        {
            //_countryRepo = countryRepo;
        }
        public ManagementCategoryBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            ManagementCategoryRepository _managementcategoryRepo = new ManagementCategoryRepository();
            return _managementcategoryRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<ManagementCategory> GetAll()
        {
            ManagementCategoryRepository _managementcategoryRepo = new ManagementCategoryRepository();
            return _managementcategoryRepo.GetAll();
        }

        public ManagementCategory GetById(int id)
        {
            ManagementCategoryRepository _managementcategoryRepo = new ManagementCategoryRepository();
            return _managementcategoryRepo.GetById(id);
        }

        public ManagementCategory Save(ManagementCategory managementcategory)
        {
            ManagementCategoryRepository _managementcategoryRepo = new ManagementCategoryRepository();
            return _managementcategoryRepo.Save(managementcategory);
        }
        public ManagementCategory Update(ManagementCategory managementcategory)
        {
            throw new NotImplementedException();
        }
    }
}
