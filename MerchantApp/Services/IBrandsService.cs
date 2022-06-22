using MerchantApp.Models;
using MerchantApp.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface IBrandsService
    {
        Brands GetById(int id);
        List<Brands> GetBySearchTerm(SearchRequest request);
        Brands Insert(BrandsInsertRequest request);
        Brands Update(int id, BrandsInsertRequest request);
        bool Delete(int id);
    }
}
