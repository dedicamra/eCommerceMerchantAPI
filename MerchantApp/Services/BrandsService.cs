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
    public class BrandsService : IBrandsService
    {
        private readonly AppDbcontext _db;
        private readonly IMapper _mapper;
        private readonly IItemService _itemService;

        private readonly UsersMerchant _currentUser;

        public BrandsService(AppDbcontext db, IMapper mapper, IItemService itemService, IHttpContextAccessor accessor)
        {
            _db = db;
            _mapper = mapper;
            _itemService = itemService;
            var username = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            _currentUser = _mapper.Map<Models.UsersMerchant>(_db.UsersMerchants.Where(x => x.Username == username).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault());
        }

        public bool Delete(int id)
        {
            var entity = _db.Brands.Find(id);
            if (entity == null)
                throw new CustomException("Brand not found.");

            _itemService.BrandGotDeleted(id);

            // _db.Remove(entity);
            entity.Active = false;
            _db.Brands.Attach(entity);
            _db.Brands.Update(entity);
            _db.SaveChanges();

            return true;
        }


        public Models.Brands GetById(int id)
        {
            var entity = _db.Brands.Where(x => x.Id == id).FirstOrDefault();

            return entity == null ? throw new CustomException("Brand not found.") : _mapper.Map<Models.Brands>(entity);
            //return _mapper.Map<Models.Brands>(entity);
        }

        //return list of categories that contains search term; if it is null it returns all 
        public List<Models.Brands> GetBySearchTerm(SearchRequest request)
        {

            var query = _db.Brands.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request?.SearchTerm))
                query = query.Where(x => x.Name.ToLower().Contains(request.SearchTerm.ToLower()));

            var list = query.ToList();
            return _mapper.Map<List<Models.Brands>>(list);
        }


        public Models.Brands Insert(BrandsInsertRequest request)
        {
            if (!Exists(request))
            {
                var entity = _mapper.Map<Data.EntityModels.Brands>(request);
                entity.Active = true;
                _db.Brands.Attach(entity);
                _db.SaveChanges();
                return _mapper.Map<Models.Brands>(entity);
            }
            throw new CustomException("Insert request not valid.");

        }


        public Models.Brands Update(int id, BrandsInsertRequest request)
        {
            
            var entity = _db.Brands.Where(x=>x.Id==id).FirstOrDefault();
            if (entity == null)
                throw new CustomException("Brand not found.");

            _db.Brands.Attach(entity);
            _db.Brands.Update(entity);

            _mapper.Map(request, entity);
            _db.SaveChanges();

            return _mapper.Map<Models.Brands>(entity);
        }



        private bool Exists(BrandsInsertRequest request)
        {
            return _db.Brands.Any(x => x.Name.ToLower() == request.Name.ToLower());
        }
    }
}

