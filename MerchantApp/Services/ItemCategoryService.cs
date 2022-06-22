using AutoMapper;
using Data;
using MerchantApp.Exceptions;
using MerchantApp.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MerchantApp.Services
{
    public class ItemCategoryService : IItemCategoryService
    {
        private readonly AppDbcontext _db;
        private readonly IMapper _mapper;
        private readonly IItemService _itemService;
        private readonly IExistsInDatabaseService _existsService;
       
        private readonly Models.UsersMerchant _currentUser;

        public ItemCategoryService(AppDbcontext db, IMapper mapper, IItemService itemService, IExistsInDatabaseService existsService, IHttpContextAccessor accessor)
        {
            _db = db;
            _mapper = mapper;
            _itemService = itemService;
            _existsService = existsService;

            var username = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            _currentUser = _mapper.Map<Models.UsersMerchant>(_db.UsersMerchants.Where(x => x.Username == username).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault());
        }

        public bool Delete(int id)
        {
            var entity = _db.ItemCategory.Find(id);
            if (entity == null)
                throw new CustomException("Category not found.");
            _itemService.ItemCategoryDeleted(id);
          
            entity.Active = false;
            _db.ItemCategory.Attach(entity);
            _db.ItemCategory.Update(entity);
            _db.SaveChanges();

            return true;
        }

      

        //returns searched category
        public Models.ItemCategory GetById(int id)
        {
            var entity = _db.ItemCategory.Where(x => x.Id == id).FirstOrDefault();

            return entity == null ? throw new CustomException("Category not found.") : _mapper.Map<Models.ItemCategory>(entity);
        }

        //returs list of categories that contains search term 
        public List<Models.ItemCategory> GetBySearchTerm(SearchRequest request)
        {

            var query = _db.ItemCategory.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request?.SearchTerm))
                query = query.Where(x => x.Name.ToLower().Contains(request.SearchTerm.ToLower()));

            var list = query.ToList();
            return _mapper.Map<List<Models.ItemCategory>>(list);
        }

        //insert new item category if it is not already in the database
        public Models.ItemCategory Insert(ItemCategoryInsertRequest request)
        {
            if (!Exists(request))
            {
                var entity = _mapper.Map<Data.EntityModels.ItemCategory>(request);
                entity.Active = true;
                _db.ItemCategory.Attach(entity);
                _db.SaveChanges();
                return _mapper.Map<Models.ItemCategory>(entity);
            }

            throw new CustomException("Insert request not valid.");
        }

        //update 
        public Models.ItemCategory Update(int id, ItemCategoryInsertRequest request)
        {
            var entity = _db.ItemCategory.Where(x => x.Id == id).FirstOrDefault();
            if (entity == null)
                throw new CustomException("Category not found.");


            _db.ItemCategory.Attach(entity);
            _db.ItemCategory.Update(entity);

            _mapper.Map(request, entity);
            _db.SaveChanges();

            return _mapper.Map<Models.ItemCategory>(entity);
        }



        private bool Exists(ItemCategoryInsertRequest request)
        {
            return _db.ItemCategory.Any(x => x.Name.ToLower() == request.Name.ToLower());
        }
    }
}
