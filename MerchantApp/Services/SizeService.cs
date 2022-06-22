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
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public class SizeService : ISizeService
    {
        private readonly AppDbcontext _db;
        private readonly IMapper _mapper;
        private readonly IExistsInDatabaseService _existService;
        private readonly IItemDetailsService _itemDetailsService;

        private readonly Models.UsersMerchant _currentUser;

        public SizeService(AppDbcontext db, IMapper mapper, IExistsInDatabaseService existService, IItemDetailsService itemDetailsService, IHttpContextAccessor accessor)
        {
            _db = db;
            _mapper = mapper;
            _existService = existService;
            _itemDetailsService = itemDetailsService;

            var username = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            _currentUser = _mapper.Map<Models.UsersMerchant>(_db.UsersMerchants.Where(x => x.Username == username).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault());
        }

        public Size GetById(int id)
        {
            var entity = _db.Size.Where(x => x.Id == id).FirstOrDefault();

            return entity == null ? throw new CustomException("Size does not exist.") : _mapper.Map<Size>(entity);
        }

        public List<Size> GetBySearchTerm(SizeSearchRequest request)
        {
            var query = _db.Size.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request?.SizeValue))
                query = query.Where(x => x.SizeValue == request.SizeValue);

            var list = query.ToList();

            return _mapper.Map<List<Size>>(list);
        }

        public Size Insert(SizeInsertRequest request)
        {
            if (Exists(request))
                throw new CustomException("Insert request not valid.");

            var entity = _mapper.Map<Data.EntityModels.Size>(request);
            entity.Active = true;
            _db.Size.Attach(entity);
            _db.SaveChanges();

            return _mapper.Map<Size>(entity);
        }


        public Size Update(int id, SizeInsertRequest request)
        {
            var entity = _db.Size.Where(x => x.Id == id).FirstOrDefault();

            if (entity == null)
                throw new CustomException("Size not found.");

            _db.Size.Attach(entity);
            _db.Size.Update(entity);

            _mapper.Map(request, entity);
            _db.SaveChanges();

            return _mapper.Map<Size>(entity);
        }

        public bool Delete(int id)
        {
            var entity = _db.Size.Where(x => x.Id == id).FirstOrDefault();
            if (entity == null)
                throw new CustomException("Size not found.");

            // implement sizeGotDeleted in itemDetails
            _itemDetailsService.SizeGotDeleted(id);

            entity.Active = false;
            _db.Size.Attach(entity);
            _db.Size.Update(entity);
            //_db.Remove(entity);
            _db.SaveChanges();

            return true;
        }


        private bool Exists(SizeInsertRequest request)
        {
            return _db.Size.Any(x => x.SizeValue == request.SizeValue);
        }
    }
}
