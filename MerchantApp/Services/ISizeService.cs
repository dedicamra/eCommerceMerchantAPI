using MerchantApp.Models;
using MerchantApp.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface ISizeService
    {
        Size GetById(int id);
        List<Size> GetBySearchTerm(SizeSearchRequest request);
        Size Insert(SizeInsertRequest request);
        Size Update(int id, SizeInsertRequest request);
        bool Delete(int id);
    }
}
