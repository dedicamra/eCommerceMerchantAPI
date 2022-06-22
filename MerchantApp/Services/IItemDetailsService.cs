using MerchantApp.Models;
using MerchantApp.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface IItemDetailsService
    {
        List<ItemDetails> GetBySearchTerm(ItemDetailsSearchRequest request);
        ItemDetails GetById(int id);
        ItemDetails Insert(ItemDetailsInsertRequest request);
        ItemDetails Update(int id, ItemDetailsInsertRequest request);
        bool Delete(int id);
        void SizeGotDeleted(int id);
        void ItemBranchGotDeleted(int id);
        void ItemGotDelete(int id);
    }
}
