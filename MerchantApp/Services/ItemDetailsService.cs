using AutoMapper;
using Data;
using MerchantApp.Exceptions;
using MerchantApp.Models;
using MerchantApp.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MerchantApp.Services
{
    public class ItemDetailsService : IItemDetailsService
    {
        private readonly AppDbcontext _db;
        private readonly IMapper _mapper;
        private readonly IExistsInDatabaseService _existService;
        private readonly IImageHelper _imageHelper;
        private readonly Models.UsersMerchant _currentUser;

        public ItemDetailsService(AppDbcontext db, IMapper mapper, IExistsInDatabaseService existService, IImageHelper imageHelper, IHttpContextAccessor accessor)
        {
            _db = db;
            _mapper = mapper;
            _existService = existService;
            _imageHelper = imageHelper;
            var username = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            _currentUser = _mapper.Map<Models.UsersMerchant>(_db.UsersMerchants.Where(x => x.Username == username).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault());
        }


        public List<ItemDetails> GetBySearchTerm(ItemDetailsSearchRequest request)
        {
            if (!ValidSearchRequest(request))
                return null;
            var query = _db.ItemDetails.Where(x => x.ItemBranch.BranchId == request.BranchId).AsQueryable();

            if (request.Active != null)
                query = query.Where(x => x.Active == request.Active);
            if (!string.IsNullOrWhiteSpace(request?.SearchTerm))
                query = query.Where(x => x.ItemBranch.Item.Name.Contains(request.SearchTerm));
            if (!string.IsNullOrWhiteSpace(request?.ItemCode))
                query = query.Where(x => x.ItemBranch.Item.Code == request.ItemCode);
            if (request.ItemCategoryId != 0)
                query = query.Where(x => x.ItemBranch.Item.ItemCategoryId == request.ItemCategoryId);

            query = query
                .Include(x => x.ItemBranch)
                .Include(x => x.ItemBranch.Item)
                .Include(x => x.Size);

            var list = query.ToList();
            foreach (var x in list)
            {
                _imageHelper.SetImageSource(x.ItemBranch.Item);
            }
            return _mapper.Map<List<ItemDetails>>(list);
        }


        public ItemDetails GetById(int id)
        {
            var entity = _db.ItemDetails.Where(x => x.Id == id)
                .Include(x => x.ItemBranch)
                .Include(x => x.ItemBranch.Item)
                .Include(x => x.Size).FirstOrDefault();
            if (entity == null)
                throw new CustomException("Item details not found.");
            _imageHelper.SetImageSource(entity.ItemBranch.Item);

            return _mapper.Map<ItemDetails>(entity);
        }


        public ItemDetails Insert(ItemDetailsInsertRequest request)
        {
            if (ValidInsertRequest(request))
            {
                var entity = _mapper.Map<Data.EntityModels.ItemDetails>(request);
                if (request.SizeId == 0)
                    entity.SizeId = null;
                entity.Active = true;

                _db.ItemDetails.Attach(entity);
                _db.SaveChanges();

                var response = _db.ItemDetails.Where(x => x.Id == entity.Id).Include(x => x.ItemBranch).ThenInclude(x => x.Item).FirstOrDefault();
                _imageHelper.SetImageSource(response.ItemBranch.Item);

                return _mapper.Map<ItemDetails>(response);
            }
            throw new CustomException("Insert request not valid.");
        }

        public ItemDetails Update(int id, ItemDetailsInsertRequest request)
        {
            var entity = _db.ItemDetails.Where(x => x.Id == id).FirstOrDefault();
            if (entity == null)
                throw new CustomException("Item details not found.");

            if (ValidInsertRequest(request))
            {
                if (request.SizeId == 0)
                    entity.SizeId = null;

                _db.ItemDetails.Attach(entity);
                _db.ItemDetails.Update(entity);

                _mapper.Map(request, entity);
                _db.SaveChanges();

                _imageHelper.SetImageSource(entity.ItemBranch.Item);

                return _mapper.Map<ItemDetails>(entity);

            }
            throw new CustomException("Update request not valid.");
        }


        public bool Delete(int id)
        {
            var entity = _db.ItemDetails.Where(x => x.Id == id).FirstOrDefault();

            if (entity == null)
                throw new CustomException("Item details not found.");

            //delete from shopping cart
            //nadji mi listu aktivnih schopping cartova
            var activeShoppingCartIds = _db.ShoppingCart.Where(x => x.Ordered == false).Select(x => x.Id).ToList();
            //lista detalja na aktivnim shopping cartovima
            var SCDetails = _db.SCDetails.Where(x => activeShoppingCartIds.Contains(x.ShoppingCartId)).AsQueryable();
            //izvuci samo one sa trenutnim detaljima
            SCDetails = SCDetails.Where(x => x.itemDetailsId == id);

            var listSCDetails = SCDetails.ToList();

            DeleteShoppingCartDetail(listSCDetails);

            entity.Active = false;
            entity.Quantity = 0;
            _db.ItemDetails.Attach(entity);
            _db.ItemDetails.Update(entity);
            _db.SaveChanges();

            return true;
        }




        public void SizeGotDeleted(int id)
        {
            var list = _db.ItemDetails.Where(x => x.SizeId == id).ToList();

            foreach (var item in list)
            {
                item.Active = false;
                _db.ItemDetails.Attach(item);
                _db.ItemDetails.Update(item);
            }
        }

        public void ItemBranchGotDeleted(int id)
        {
            var itemDetails = _db.ItemDetails.Where(x => x.ItemBranchId == id).ToList(); 
            var itemDetailsIds = itemDetails.Select(x => x.Id).ToList();

            //nadji mi listu aktivnih schopping cartova
            var activeShoppingCartIds = _db.ShoppingCart.Where(x => x.Ordered == false).Select(x => x.Id).ToList();
            //lista detalja na aktivnim shopping cartovima
            var SCDetails = _db.SCDetails.Where(x => activeShoppingCartIds.Contains(x.ShoppingCartId)).AsQueryable();
            //izvuci samo one sa trenutnim detaljima
            SCDetails = SCDetails.Where(x => itemDetailsIds.Contains(x.itemDetailsId));

            var listSCDetails = SCDetails.ToList();

            DeleteShoppingCartDetail(listSCDetails);
            foreach (var item in itemDetails)
            {
                item.Active = false;
                _db.ItemDetails.Attach(item);
                _db.ItemDetails.Update(item);
            }
        }

        public void ItemGotDelete(int id)
        {
            var itemDetails = _db.ItemDetails.Where(x => x.ItemBranch.ItemId == id).ToList();
            var itemDetailsIds = itemDetails.Select(x => x.Id).ToList();

            //nadji mi listu aktivnih schopping cartova
            var activeShoppingCartIds = _db.ShoppingCart.Where(x => x.Ordered == false).Select(x => x.Id).ToList();
            //lista detalja na aktivnim shopping cartovima
            var SCDetails = _db.SCDetails.Where(x => activeShoppingCartIds.Contains(x.ShoppingCartId)).AsQueryable();
            //izvuci samo one sa trenutnim detaljima
            SCDetails = SCDetails.Where(x => itemDetailsIds.Contains(x.itemDetailsId));

            var listSCDetails = SCDetails.ToList();

            DeleteShoppingCartDetail(listSCDetails);
            foreach (var item in itemDetails)
            {
                item.Active = false;
                _db.ItemDetails.Attach(item);
                _db.ItemDetails.Update(item);
            }
        }

        private void DeleteShoppingCartDetail(List<Data.EntityModels.SCDetails> listSCDetails)
        {
            foreach (var scd in listSCDetails)
            {
                _db.Remove(scd);
            }
            _db.SaveChanges();
        }


        private bool ValidInsertRequest(ItemDetailsInsertRequest request)
        {
            if (!_existService.ItemBranchExists(request.ItemBranchId))
                return false;
            if (request.SizeId != 0 && !_existService.SizeExists(request.SizeId))
                return false;
            return true;
        }
        private bool ValidSearchRequest(ItemDetailsSearchRequest request)
        {
            if (request.BranchId == 0)
                return false;
            if (!_existService.BranchExists(request.BranchId))
                return false;
            if (request.ItemCategoryId != 0 && !_existService.ItemCategoryExists(request.ItemCategoryId))
                return false;
            return true;
        }

    }
}
