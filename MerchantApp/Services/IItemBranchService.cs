using MerchantApp.Models;
using MerchantApp.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface IItemBranchService
    {
        List<ItemBranch> GetBySearchTerm(ItemBranchSearchRequest request);
       
        ItemBranch GetById(int id);
        ItemBranch Insert(ItemBranchInsertRequest request);
        ItemBranch Update(int id, ItemBranchInsertRequest request);
        bool Delete(int id);


        void ItemGotDeleted(int Id);
        void ItemEdited(int Id, string name);
        void BranchGotDeleted(int Id);
        void BranchEdited(int Id, string name);
        //void BrandGotDeleted(int Id);
    }
}
