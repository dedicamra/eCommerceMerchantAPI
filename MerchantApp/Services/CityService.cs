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
    public class CityService:ICityService
    {
        private readonly AppDbcontext _db;
        private readonly IMapper _mapper;
        private readonly IBranchesService _branchService;
        private readonly IExistsInDatabaseService _existsService;

        private readonly UsersMerchant _currentUser;

        public CityService(AppDbcontext db, IMapper mapper, IBranchesService branchService, IExistsInDatabaseService existsService, IHttpContextAccessor accessor)
        {
            _db = db;
            _mapper = mapper;
            _branchService = branchService;
            _existsService = existsService;

            var username = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            _currentUser = _mapper.Map<Models.UsersMerchant>(_db.UsersMerchants.Where(x => x.Username == username).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault());
        }


        public bool Delete(int id)
        {
       
            var entity = _db.City.Find(id);
            if (entity == null)
                throw new CustomException("City not found.");

            //if city gets deleted set active to false for all branches in that city  
            _branchService.CityDelete(id);

            entity.Active = false;
            _db.City.Attach(entity);
            _db.City.Update(entity);
           
            _db.SaveChanges();

            return true;
        }


        public Models.City GetById(int id)
        {
            var entity = _db.City.Where(x => x.Id == id).FirstOrDefault();
            return entity==null? throw new CustomException("City not found."): _mapper.Map<Models.City>(entity);
        }

        
        public List<Models.City> GetBySearchTerm(SearchRequest request)
        {

            var query = _db.City.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request?.SearchTerm))
                query =query.Where(x => x.Name.ToLower().Contains(request.SearchTerm.ToLower()));
            //if (request.Active!=null)
            //    query = query.Where(x => x.Active==request.Active);

            var list = query.ToList();
            return _mapper.Map<List<Models.City>>(list);
        }

      
        public Models.City Insert(CityInsertRequest request)
        {
            if (!Exists(request))
            {
                var entity = _mapper.Map<Data.EntityModels.City>(request);
                entity.Active = true;
                _db.City.Attach(entity);
                _db.SaveChanges();
                return _mapper.Map<Models.City>(entity);
            }

            throw new CustomException("Insert request not valid.");
        }

        //update 
        public Models.City Update(int id, CityInsertRequest request)
        {
            var entity = _db.City.Where(x => x.Id == id).FirstOrDefault();
            if (entity==null)
                throw new CustomException("City not found.");

            _db.City.Attach(entity);
            _db.City.Update(entity);

            _mapper.Map(request, entity);
            
            _branchService.CityEdited(id, request.Name);

            _db.SaveChanges();

            return _mapper.Map<Models.City>(entity);
        }



        private bool Exists(CityInsertRequest request)
        {
            return _db.City.Any(x => x.Name == request.Name);
        }
    }
}
