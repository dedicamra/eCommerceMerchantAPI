using MerchantApp.Models;
using MerchantApp.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface IItemCategoryService
    {
        //List<ItemCategory> Get();
        ItemCategory GetById(int id);
        List<ItemCategory> GetBySearchTerm(SearchRequest request);
        ItemCategory Insert(ItemCategoryInsertRequest request);
        ItemCategory Update(int id, ItemCategoryInsertRequest request);
        bool Delete(int id);
    }
}
