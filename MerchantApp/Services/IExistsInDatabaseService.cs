using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface IExistsInDatabaseService
    {
        bool BranchExists(int Id);
        bool BrandExists(int Id);
        bool CityExists(int Id);
        bool GenderExists(int Id);
        bool ItemBranchExists(int Id);
        bool ItemBranchExistsInsert(int itemId, int BranchId);
        bool ItemCategoryExists(int Id);
        bool ItemDetailsExists(int Id);
        bool ItemExists(int Id);
        bool SizeExists(int Id);
        bool CouponIdExists(int Id);
        bool CouponCodeExists(string code);
    }
}
