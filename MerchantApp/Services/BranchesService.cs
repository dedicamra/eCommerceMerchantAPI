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
    public class BranchesService : IBranchesService
    {
        private readonly AppDbcontext _db;
        private readonly IMapper _mapper;
        private readonly IItemBranchService _itemBranchService;
        private readonly IExistsInDatabaseService _existService;

        private readonly Models.UsersMerchant _currentUser;

        public BranchesService(AppDbcontext db, IMapper mapper, IItemBranchService itemBranchService, IExistsInDatabaseService existService, IHttpContextAccessor accessor)
        {
            _db = db;
            _mapper = mapper;
            _itemBranchService = itemBranchService;
            _existService = existService;

            var username = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            _currentUser = _mapper.Map<Models.UsersMerchant>(_db.UsersMerchants.Where(x => x.Username == username).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault());

        }


        public List<Branches> GetBySearchTerm(SearchRequest request)
        {
            var query = _db.Branches.AsQueryable();

            if (request.Active != null)
                query = query.Where(x => x.Active == request.Active);
            if (!string.IsNullOrWhiteSpace(request?.SearchTerm))
                query = query.Where(x => x.Name.ToLower().Contains(request.SearchTerm.ToLower()));
            if (!string.IsNullOrWhiteSpace(request?.Adress))
                query = query.Where(x => x.Adress.ToLower().Contains(request.Adress.ToLower()));

            query = query.Include(x => x.City);

            var list = query.ToList();
            return _mapper.Map<List<Models.Branches>>(list);
        }

        public Branches GetById(int id)
        {
            var entity = _db.Branches.Where(x => x.Id == id).Include(x => x.City).FirstOrDefault();

            return entity == null ? throw new CustomException("Branch not found.") : _mapper.Map<Models.Branches>(entity);
        }

        public Branches Insert(BranchInsertRequest request)
        {
            if (ValidRequest(request) && !Exists(request))
            {
                var entity = _mapper.Map<Data.EntityModels.Branches>(request);
                
                _db.Branches.Attach(entity);
                _db.SaveChanges();
                var response = _db.Branches.Where(x => x.Id == entity.Id).Include(x => x.City).FirstOrDefault();

                //_mapper.Map(request, entity);
                return _mapper.Map<Models.Branches>(response);
            }
            throw new CustomException("Insert request not valid.");
        }

        public Branches Update(int id, BranchInsertRequest request)
        {
            //check if the id exists in db
            if (!_existService.BranchExists(id))
                throw new CustomException("Branch not found.");

            if (!ValidRequest(request))
                throw new CustomException("Update request not valid.");

            var entity = _db.Branches.Find(id);

            _db.Branches.Attach(entity);
            _db.Branches.Update(entity);


            _mapper.Map(request, entity);

            var response = _db.Branches.Where(x => x.Id == entity.Id).Include(x => x.City).FirstOrDefault();

            _db.SaveChanges();

            return _mapper.Map<Models.Branches>(response);
        }


        public bool Delete(int id)
        {
            if (!_existService.BranchExists(id))
                throw new CustomException("Branch does not exist.");
            var entity = _db.Branches.Find(id);
            entity.Active = false;
            //delete itembranch
            _itemBranchService.BranchGotDeleted(id);

            _db.Branches.Attach(entity);
            _db.Branches.Update(entity);
            _db.SaveChanges();

            return true;
        }



        private bool Exists(BranchInsertRequest request)
        {
            return _db.Branches.Any(x => x.Name.ToLower() == request.Name.ToLower() && x.CityId == request.CityId && x.Adress == request.Adress);
        }

        private bool ValidRequest(BranchInsertRequest request)
        {
            if (!_existService.CityExists(request.CityId))
                return false;
            return true;
        }

        public void CityDelete(int id)
        {
            var list = _db.Branches.Where(x => x.CityId == id).ToList();
            foreach (var branch in list)
            {
                _itemBranchService.BranchGotDeleted(branch.Id);
                branch.Active = false;
               
                _db.Branches.Attach(branch);
                _db.Branches.Update(branch);
            }

        }
        public void CityEdited(int id, string name)
        {
            var list = _db.Branches.Where(x => x.CityId == id).ToList();

            foreach (var branch in list)
            {
                branch.CityName = name;
                _db.Branches.Attach(branch);
                _db.Branches.Update(branch);
            }

        }
    }
}
