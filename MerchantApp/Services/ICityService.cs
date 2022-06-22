using MerchantApp.Models;
using MerchantApp.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface ICityService
    {
        City GetById(int id);
        List<City> GetBySearchTerm(SearchRequest request);
        City Insert(CityInsertRequest request);
        City Update(int id, CityInsertRequest request);
        bool Delete(int id);
    }
}
