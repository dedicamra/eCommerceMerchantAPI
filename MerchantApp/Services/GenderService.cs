using AutoMapper;
using Data;
using Data.EntityModels;
using MerchantApp.Exceptions;
using MerchantApp.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public class GenderService : IGenderService
    {

        private readonly AppDbcontext _db;
        private readonly IMapper _mapper;

        private readonly Models.UsersMerchant _currentUser;

        public GenderService(AppDbcontext db, IMapper mapper, IHttpContextAccessor accessor)
        {
            _db = db;
            _mapper = mapper;

            var username = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            _currentUser = _mapper.Map<Models.UsersMerchant>(_db.UsersMerchants.Where(x => x.Username == username).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault());
        }

        public bool Delete(int id)
        {
            var entity = _db.Gender.Find(id);
            if (entity == null)
                throw new CustomException("Gender not found.");
            
            entity.Active = false;
            _db.Gender.Attach(entity);
            _db.Gender.Update(entity);
            _db.SaveChanges();

            return true;
        }

        public Models.Gender GetById(int id)
        {
            var entity = _db.Gender.Where(x => x.Id == id).FirstOrDefault();
            return entity == null ? throw new CustomException("Gender not found.") : _mapper.Map<Models.Gender>(entity);
        }

        public List<Models.Gender> GetBySearchTerm(SearchRequest request)
        {
            var query = _db.Gender.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request?.SearchTerm))
                query = _db.Gender.Where(x => x.Name.ToLower().Contains(request.SearchTerm.ToLower()));

            var list = query.ToList();
            return _mapper.Map<List<Models.Gender>>(list);
        }

        public Models.Gender Insert(GenderInsertRequest request)
        {
            if (!Exists(request.Name))
            {

            var entity = _mapper.Map<Data.EntityModels.Gender>(request);
            entity.Active = true;

            _db.Gender.Attach(entity);
            _db.SaveChanges();
            return _mapper.Map<Models.Gender>(entity);
            }
            throw new CustomException("Insert request not valid");
        }
        public Models.Gender Update(int id, GenderInsertRequest request)
        {
            var entity = _db.Gender.Where(x=>x.Id==id).FirstOrDefault();
            if (entity == null)
                throw new CustomException("Gender not found.");

            _db.Gender.Attach(entity);
            _db.Gender.Update(entity);

            _mapper.Map(request, entity);
            _db.SaveChanges();

            return _mapper.Map<Models.Gender>(entity);
        }

        private bool Exists(string name)
        {
            return _db.Gender.Any(x => x.Name.ToLower()== name.ToLower());
        }

    }
}
