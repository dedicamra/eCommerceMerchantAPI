using MerchantApp.Models;
using MerchantApp.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface IBranchesService
    {
        List<Branches> GetBySearchTerm(SearchRequest request);
        //List<Branches> GetBySearchTerm(SearchRequest request, bool active);
        Branches GetById(int id);
        Branches Insert(BranchInsertRequest request);
        Branches Update(int id, BranchInsertRequest request);
        //string Update(int id, BranchInsertRequest request);
        bool Delete(int id);
        void CityDelete(int id);
        public void CityEdited(int Id, string name);

    }
}
