using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public class ExistsInDatabaseService : IExistsInDatabaseService
    {
        private readonly AppDbcontext _db;

        public ExistsInDatabaseService(AppDbcontext db)
        {
            _db = db;
        }

        public bool BranchExists(int Id)
        {
            return _db.Branches.Any(x=>x.Id==Id);
        }

        public bool BrandExists(int Id)
        {
            return _db.Brands.Any(x=>x.Id==Id);
        }

        public bool CityExists(int Id)
        {
            return _db.City.Any(x => x.Id == Id);

        }

        public bool CouponCodeExists(string code)
        {
            return _db.Coupons.Any(x => x.Code == code);
        }

        public bool CouponIdExists(int Id)
        {
            return _db.Coupons.Any(x => x.Id == Id);
        }

        public bool GenderExists(int Id)
        {
            return _db.Gender.Any(x => x.Id == Id);

        }

        public bool ItemBranchExists(int Id)
        {
            return _db.ItemBranch.Any(x => x.Id == Id);

        }

        public bool ItemBranchExistsInsert(int itemId, int BranchId)
        {
            var list = _db.ItemBranch.Where(x => x.BranchId == BranchId).ToList();
            foreach (var item in list)
            {
                if (item.ItemId == itemId)
                    return true;
            }
            return false;
        }

        public bool ItemCategoryExists(int Id)
        {
            return _db.ItemCategory.Any(x => x.Id == Id);

        }

        public bool ItemDetailsExists(int Id)
        {
            return _db.ItemDetails.Any(x => x.Id == Id);

        }

        public bool ItemExists(int Id)
        {
            return _db.Items.Any(x => x.Id == Id);

        }

        public bool SizeExists(int Id)
        {
            return _db.Size.Any(x => x.Id == Id);

        }
    }
}
