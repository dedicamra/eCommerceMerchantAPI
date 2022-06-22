using AutoMapper;
using Data;
using MerchantApp.Exceptions;
using MerchantApp.Models;
using MerchantApp.Requests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MerchantApp.Services
{
    public class ItemService : IItemService
    {
        private readonly AppDbcontext _db;
        private readonly IMapper _mapper;
        private readonly IImageHelper _imageHelper;
        private readonly IItemBranchService _itemBranchService;
        private readonly IExistsInDatabaseService _existService;
        private readonly IWebHostEnvironment _hostEnviroment;
        //private readonly string _path = "https://localhost:44370/Images/";
        //private readonly string __path = $"{Request.Headers["origin"]}"


        private readonly Models.UsersMerchant _currentUser;

        public ItemService(AppDbcontext db, IMapper mapper, IImageHelper imageHelper, IItemBranchService itemBranchService, IExistsInDatabaseService existService, IWebHostEnvironment hostEnviroment, IHttpContextAccessor accessor)
        {
            _db = db;
            _mapper = mapper;
            _imageHelper = imageHelper;
            _itemBranchService = itemBranchService;
            _existService = existService;
            _hostEnviroment = hostEnviroment;


            var username = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            _currentUser = _mapper.Map<Models.UsersMerchant>(_db.UsersMerchants.Where(x => x.Username == username).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault());
        }

        public Items GetById(int id)
        {

            var entity = _db.Items.Where(x => x.Id == id)
                              .Include(x => x.Brand)
                              .Include(x => x.ItemCategory)
                              .Include(x => x.Gender).FirstOrDefault();
            if (entity == null)
                throw new CustomException("Item not found.");

            //if (entity.ImageName != null)
            //    entity.ImageSource = _path + entity.ImageName;
            //else
            //    entity.ImageSource = _path + "noimageavailable.png";
            _imageHelper.SetImageSource(entity);

            return _mapper.Map<Models.Items>(entity);
        }

        public List<Items> GetBySearchTerm(ItemSearchRequest request)
        {
            var query = _db.Items.AsQueryable();

            if (request != null)
                query = query.Where(x => x.Active == request.Active);

            if (!string.IsNullOrWhiteSpace(request?.Name))
                query = query.Where(x => x.Name.ToLower().Contains(request.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(request?.Code))
                query = query.Where(x => x.Code == request.Code);

            if (request.ItemCategoryId != null)
                query = query.Where(x => x.ItemCategoryId == request.ItemCategoryId);

            if (request.BrandId != null)
                query = query.Where(x => x.BrandId == request.BrandId);

            if (request.GenderId != null)
                query = query.Where(x => x.GenderId == request.GenderId);

            query = query
                    .Include(x => x.ItemCategory)
                    .Include(x => x.Brand)
                    .Include(x => x.Gender);

            var list = query.ToList();


            foreach (var item in list)
            {
                _imageHelper.SetImageSource(item);
                //if (item.ImageName != null)
                //    item.ImageSource = _path + item.ImageName;
                //else
                //    item.ImageSource = _path + "noimageavailable.png";
            }

            return _mapper.Map<List<Models.Items>>(list);
        }

        public Items Insert(ItemInsertRequest request)
        {
            if (!Exists(request) && ValidRequest(request))
            {
                request.ImageName = _imageHelper.SaveImage(request.ImageFile);
                var entity = _mapper.Map<Data.EntityModels.Items>(request);
                entity.BrandName = _db.Brands.Find(request.BrandId).Name;
                entity.GenderName = _db.Gender.Find(request.GenderId).Name;
                entity.CategoryName = _db.ItemCategory.Find(request.ItemCategoryId).Name;
                entity.Active = true;
                entity.Price = CalculatePrice(request.StartPrice, request.Discount);
                if (request.ImageFile != null)
                {
                    MemoryStream ms = new MemoryStream();
                    request.ImageFile.CopyTo(ms);
                    entity.ImageByteArray = ms.ToArray();
                }

                _db.Items.Attach(entity);
                _db.SaveChanges();


                _imageHelper.SetImageSource(entity);
                var result = _mapper.Map<Models.Items>(entity);

                return result;
            }
            throw new CustomException("Insert request not valid.");
        }


        public Items Update(int id, ItemUpdateRequest request)
        {
            var entity = _db.Items.Where(x => x.Id == id).FirstOrDefault();
            if (entity == null)
                throw new CustomException("Item not found.");

            if (ValidRequest(request))
            {
                entity.BrandName = _db.Brands.Find(request.BrandId).Name;
                entity.CategoryName = _db.ItemCategory.Find(request.ItemCategoryId).Name;
                entity.GenderName = _db.Gender.Find(request.GenderId).Name;
                if (request.ImageFile != null)
                {
                    _imageHelper.DeleteImage(entity.ImageName);
                    request.ImageName = _imageHelper.SaveImage(request.ImageFile);
                    entity.ImageName = request.ImageName;

                    MemoryStream ms = new MemoryStream();
                    request.ImageFile.CopyTo(ms);
                    entity.ImageByteArray = ms.ToArray();
                }
                else
                {
                    request.ImageName = _db.Items.Where(x => x.Id == id).FirstOrDefault().ImageName;
                }
                entity.Price = CalculatePrice(request.StartPrice, request.Discount);

                _db.Items.Attach(entity);
                _db.Items.Update(entity);

                _mapper.Map(request, entity);
                _db.SaveChanges();

                _imageHelper.SetImageSource(entity);
                var result = _mapper.Map<Models.Items>(entity);

              
                return result;
            }
            throw new CustomException("Update request not valid.");
        }



        private float CalculatePrice(float startPrice, float discount)
        {
            var disc = startPrice * (discount / 100);
            var result = startPrice - disc;
            return result;
        }

        //Delete

        public void ItemCategoryDeleted(int id)
        {
            var list = _db.Items.Where(x => x.ItemCategoryId == id).ToList();
            foreach (var item in list)
            {
                //item.ItemCategoryId = null;
                //_db.Items.Attach(item);
                //_db.Items.Update(item);
                Delete(item.Id);
            }

            _db.SaveChanges();
        }
        public void BrandGotDeleted(int id)
        {
            var list = _db.Items.Where(x => x.BrandId == id).ToList();
            foreach (var item in list)
            {
                _itemBranchService.ItemGotDeleted(item.Id);
                item.BrandId = null;
                _db.Items.Attach(item);
                _db.Items.Update(item);
            }

            //_db.SaveChanges();
        }

        public bool Delete(int id)
        {
            var entity = _db.Items.Where(x => x.Id == id).FirstOrDefault();
            if (entity == null)
                throw new CustomException("Item not found.");

            _itemBranchService.ItemGotDeleted(id);
            //DeleteShoppingCartDetails(id);
            entity.Active = false;
            _db.Items.Attach(entity);
            _db.Items.Update(entity);
            //_db.Remove(entity);
            _db.SaveChanges();

            return true;
        }



        private bool Exists(ItemInsertRequest request)
        {
            return _db.Items.Any(x => x.Code == request.Code);
        }

        private bool ValidRequest(ItemUpdateRequest request)
        {
            if (!_existService.ItemCategoryExists(request.ItemCategoryId) ||
                !_existService.BrandExists(request.BrandId) ||
                !_existService.GenderExists(request.GenderId))
                return false;

            return true;
        }
        private bool ValidRequest(ItemInsertRequest request)
        {
            if (!_existService.ItemCategoryExists(request.ItemCategoryId) ||
                !_existService.BrandExists(request.BrandId) ||
                !_existService.GenderExists(request.GenderId))
                return false;

            return true;
        }



    }
}
