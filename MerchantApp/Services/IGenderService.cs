using MerchantApp.Models;
using MerchantApp.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface IGenderService
    {
        Gender GetById(int id);
        List<Gender> GetBySearchTerm(SearchRequest request);
        Gender Insert(GenderInsertRequest request);
        Gender Update(int id, GenderInsertRequest request);
        bool Delete(int id);
    }
}
