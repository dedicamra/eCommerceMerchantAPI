using AutoMapper;
using Data;
using Data.EntityModels;
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
    public class UserProfileService:IUserProfileService
    {
        private readonly AppDbcontext _db;
        private readonly IMapper _mapper;

        private readonly Data.EntityModels.UsersMerchant _currentUser;

        public UserProfileService(AppDbcontext db, IMapper mapper, IHttpContextAccessor accessor)
        {
            _db = db;
            _mapper = mapper;

            var username = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            _currentUser = _db.UsersMerchants.Where(x => x.Username == username).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault();
        }

        public UserProfile EditProfile(UserUpdateRequest request)
        {
            
            if (_currentUser.Username != request.Username && CheckUsernameExists(request.Username))
            {
                throw new CustomException("Username is already taken.");
            }
            if (_currentUser.Email != request.Email && CheckEmailExists(request.Email))
            {
                throw new CustomException("Email is already taken.");
            }

            _db.UsersMerchants.Attach(_currentUser);
            _db.UsersMerchants.Update(_currentUser);

            _mapper.Map(request, _currentUser);
            _db.SaveChanges();

            return _mapper.Map<UserProfile>(_currentUser);
        }


        public bool CheckUsernameExists(string username)
        {
            return _db.UsersMerchants.Any(x => x.Username == username);
        }
        public bool CheckEmailExists(string email)
        {
            return _db.UsersMerchants.Any(x => x.Email == email);
        }
    }
}
