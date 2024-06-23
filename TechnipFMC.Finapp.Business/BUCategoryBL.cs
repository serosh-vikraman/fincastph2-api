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
    public class BUCategoryBL : IBUCategoryBL
    {
        public BUCategoryBL(IBUCategoryRepository buCategoryRepo)
        {
            //_countryRepo = countryRepo;
        }
        public BUCategoryBL()
        { }

        public bool Delete(int Id, string Deletedby)
        {
            BUCategoryRepository _buCategoryRepo = new BUCategoryRepository();
            return _buCategoryRepo.Delete(Id, Deletedby);
        }

        public IEnumerable<BUCategory> GetAll()
        {
            BUCategoryRepository _buCategoryRepo = new BUCategoryRepository();
            return _buCategoryRepo.GetAll();
        }

        public BUCategory GetById(int id)
        {
            BUCategoryRepository _buCategoryRepo = new BUCategoryRepository();
            return _buCategoryRepo.GetById(id);
        }

        public BUCategory Save(BUCategory buCategory)
        {
            BUCategoryRepository _buCategoryRepo = new BUCategoryRepository();
            return _buCategoryRepo.Save(buCategory);
        }
        public BUCategory Update(BUCategory buCategory)
        {
            throw new NotImplementedException();
        }
    }
}
