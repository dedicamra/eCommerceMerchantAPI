using MerchantApp.Models;
using MerchantApp.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface IItemService
    {
        List<Items> GetBySearchTerm(ItemSearchRequest request);
        Items GetById(int id);
        Items Insert(ItemInsertRequest request);
        Items Update(int id, ItemUpdateRequest request);

        bool Delete(int id);
        void ItemCategoryDeleted(int id);
        void BrandGotDeleted(int id);


        //void SetImageSource(Data.EntityModels.Items result);
    }
}
