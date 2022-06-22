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
    public class CouponService : ICouponService
    {
        private readonly AppDbcontext _db;
        private readonly IMapper _mapper;
        private readonly IExistsInDatabaseService _existService;
        private readonly IUserService _userService;

        private readonly UsersMerchant _currentUser;

        public CouponService(AppDbcontext db, IMapper mapper, IExistsInDatabaseService existService, IUserService userService, IHttpContextAccessor accessor)
        {
            _db = db;
            _mapper = mapper;
            _existService = existService;
            _userService = userService;

            var username = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            _currentUser = _mapper.Map<Models.UsersMerchant>(_db.UsersMerchants.Where(x => x.Username == username).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault());

        }

        public bool Delete(int id)
        {
            if (_existService.CouponIdExists(id))
            {
                var entity = _db.Coupons.Where(x => x.Id == id).FirstOrDefault();
                entity.Active = false;
                _db.Coupons.Attach(entity);
                _db.Coupons.Update(entity);

                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public Coupons GetById(int id)
        {
            var entity = _db.Coupons.Where(x => x.Id == id).FirstOrDefault();

            return entity == null ? throw new CustomException("Coupon not found.") : _mapper.Map<Coupons>(entity);

        }

        public List<Coupons> GetBySearchTerm(CouponSearchRequest request)
        {
            var query = _db.Coupons.Where(x => x.Active == request.Active).AsQueryable();

            if (!string.IsNullOrWhiteSpace(request?.Code))
                query = query.Where(x => x.Code == request.Code);
            if (request.ValidUntil != null)
                query = query.Where(x => x.ValidUntil <= request.ValidUntil);
            if (request.Discount != 0)
                query = query.Where(x => x.Discount == request.Discount);

           

            var list = query.ToList();
            return _mapper.Map<List<Coupons>>(list);
        }

        public Coupons Insert(CouponInsertRequest request)
        {
            if (!_existService.CouponCodeExists(request.Code))
            {
                var entity = _mapper.Map<Data.EntityModels.Coupons>(request);
                entity.UsersMerchantId = _currentUser.Id;
                entity.CreatedOn = DateTime.Today;
                entity.Active = true;
                _db.Coupons.Attach(entity);
                _db.SaveChanges();
                return _mapper.Map<Models.Coupons>(entity);
            }

            throw new CustomException("Coupon code already exists.");
        }


        public Coupons Update(int id, CouponInsertRequest request)
        {

            var entity = _db.Coupons.Where(x => x.Id == id).FirstOrDefault();
            if (entity == null)
                throw new CustomException("Coupon not found.");

            entity.UsersMerchantId = _currentUser.Id;
            _db.Coupons.Attach(entity);
            _db.Coupons.Update(entity);

            _mapper.Map(request, entity);

            _db.SaveChanges();

            return _mapper.Map<Models.Coupons>(entity);
        }



        private bool ValidInsertRequest(CouponInsertRequest request)
        {
            return _existService.CouponCodeExists(request.Code);
        }
    }
}
