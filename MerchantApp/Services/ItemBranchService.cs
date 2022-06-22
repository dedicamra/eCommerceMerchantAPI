using AutoMapper;
using Data;
using MerchantApp.Exceptions;
using MerchantApp.Models;
using MerchantApp.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MerchantApp.Services
{
    public class ItemBranchService : IItemBranchService
    {
        private readonly AppDbcontext _db;
        private readonly IMapper _mapper;
        private readonly IItemDetailsService _itemDetailsService;
        private readonly IImageHelper _imageHelper;
        private readonly IExistsInDatabaseService _existsService;
        
        private readonly Models.UsersMerchant _currentUser;

        public ItemBranchService(AppDbcontext db, IMapper mapper, IItemDetailsService itemDetailsService, IImageHelper imageHelper, IExistsInDatabaseService existsService, IHttpContextAccessor accessor)
        {
            _db = db;
            _mapper = mapper;
            _itemDetailsService = itemDetailsService;
            _imageHelper = imageHelper;
            _existsService = existsService;

            var username = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            _currentUser = _mapper.Map<Models.UsersMerchant>(_db.UsersMerchants.Where(x => x.Username == username).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault());

        }

        public bool Delete(int id)
        {
            var entity = _db.ItemBranch.Where(x => x.Id == id).FirstOrDefault();

            if (entity == null)
                throw new CustomException("Item not found.");

            CheckIfUserHasPermission(_currentUser, entity.BranchId);

            //brisanje svih aktivnih scDetailsa 
            _itemDetailsService.ItemBranchGotDeleted(entity.Id);

           
            entity.Active = false;
            _db.ItemBranch.Attach(entity);
            _db.ItemBranch.Update(entity);
            _db.SaveChanges();
            return true;
        }

        private void CheckIfUserHasPermission(UsersMerchant currentUser, int? branchId)
        {
            if (_currentUser.Role.Name.Equals("Merchant") && _currentUser.Branch.Id != branchId)
                throw new CustomException("Access denied.");
        }

        public ItemBranch GetById(int id)
        {
            var entity = _db.ItemBranch.Where(x => x.Id == id)
                .Include(x => x.Item)
                .Include(x => x.Branch)
                .FirstOrDefault();
            if (entity == null)
                throw new CustomException("Item not found.");
            _imageHelper.SetImageSource(entity.Item);

            return _mapper.Map<Models.ItemBranch>(entity);
        }

        public List<ItemBranch> GetBySearchTerm(ItemBranchSearchRequest request)
        {
            var query = _db.ItemBranch.Where(x => x.Active == true).AsQueryable();

            if (request.BranchId != 0)
                query = query.Where(x => x.BranchId == request.BranchId);

            if (!string.IsNullOrWhiteSpace(request?.SearchByItemCode))
                query = query.Where(x => x.ItemCode == request.SearchByItemCode);
            if (request.FirstDate != null)
                query = query.Where(x => x.DateOfAdding >= request.FirstDate);
            if (request.SecondDate != null)
                query = query.Where(x => x.DateOfAdding <= request.SecondDate);
            if (request.ItemCategoryId != 0)
                query = query.Where(x => x.Item.ItemCategoryId == request.ItemCategoryId);
            if (request.GenderId != 0)
                query = query.Where(x => x.Item.GenderId == request.GenderId);
            if (request.BrandId != 0)
                query = query.Where(x => x.Item.BrandId == request.BrandId);

            query = query.Include(x => x.Branch)
                    .Include(x => x.Item);

            var list = query.ToList();
            foreach (var x in list)
            {
                _imageHelper.SetImageSource(x.Item);
            }
            return _mapper.Map<List<Models.ItemBranch>>(list);
        }

        public ItemBranch Insert(ItemBranchInsertRequest request)
        {
            CheckIfUserHasPermission(_currentUser, request.BranchId);

            if (ValidInsertRequest(request))
            {
                var entity = _mapper.Map<Data.EntityModels.ItemBranch>(request);
                var _branch = _db.Branches.Find(request.BranchId);
                var _item = _db.Items.Find(request.ItemId);
                entity.BranchName = $"{_branch.Name}, {_branch.Adress}, {_branch.CityName}";
                entity.ItemCode = _item.Code;
                entity.Active = true;

                _db.ItemBranch.Attach(entity);
                _db.SaveChanges();

                _imageHelper.SetImageSource(entity.Item);

                return _mapper.Map<Models.ItemBranch>(entity);
            }
            throw new CustomException("Insert request not valid.");

        }

        public ItemBranch Update(int id, ItemBranchInsertRequest request)
        {
            CheckIfUserHasPermission(_currentUser,request.BranchId);

            var entity = _db.ItemBranch.Where(x => x.Id == id).FirstOrDefault();
            if (entity == null)
                throw new CustomException("Item on branch not found");

            var _branch = _db.Branches.Where(x => x.Id == request.BranchId).FirstOrDefault();
            if (_branch == null)
                throw new CustomException("Branch not found.");

            var _item = _db.Items.Find(request.ItemId);
            if (_item == null)
                throw new CustomException("Item not found.");

            entity.BranchName = $"{_branch.Name}, {_branch.Adress}, {_branch.CityName}";
            entity.ItemCode = _item.Code;

            _db.ItemBranch.Attach(entity);
            _db.ItemBranch.Update(entity);

            _mapper.Map(request, entity);
            
            _db.SaveChanges();
            
            _imageHelper.SetImageSource(entity.Item);

            return _mapper.Map<ItemBranch>(entity);
        }

        
        
        
        private bool ValidInsertRequest(ItemBranchInsertRequest request)
        {
            if (!_existsService.BranchExists(request.BranchId))
                return false;
            if (!_existsService.ItemExists(request.ItemId))
                return false;
            if (_existsService.ItemBranchExistsInsert(request.ItemId, request.BranchId))
                return false;
            return true;
        }
        
        public void ItemGotDeleted(int Id)
        {
            _itemDetailsService.ItemGotDelete(Id);

            var list = _db.ItemBranch.Where(x => x.Item.Id == Id).ToList();
            foreach (var item in list)
            {
                item.Active = false;
                _db.ItemBranch.Attach(item);
                _db.ItemBranch.Update(item);
            }


        }
        public void BranchGotDeleted(int Id)
        {
            var list = _db.ItemBranch.Where(x => x.Branch.Id == Id).ToList();
            foreach (var item in list)
            {
                _itemDetailsService.ItemBranchGotDeleted(item.Id);

                item.Active = false;
                _db.ItemBranch.Attach(item);
                _db.ItemBranch.Update(item);
            }


        }

        public void ItemEdited(int Id, string name)
        {
            var list = _db.ItemBranch.Where(x => x.Item.Id == Id).ToList();
            foreach (var item in list)
            {
                item.ItemCode = name;
                _db.ItemBranch.Attach(item);
                _db.ItemBranch.Update(item);
            }

        }

        public void BranchEdited(int Id, string name)
        {
            throw new NotImplementedException();
        }
    }
}
