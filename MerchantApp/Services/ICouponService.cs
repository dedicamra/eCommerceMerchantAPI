using MerchantApp.Models;
using MerchantApp.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface ICouponService
    {
        Coupons GetById(int id);
        List<Coupons> GetBySearchTerm(CouponSearchRequest request);
        Coupons Insert(CouponInsertRequest request);
        Coupons Update(int id, CouponInsertRequest request);
        bool Delete(int id);
    }
}
