using MerchantApp.Exceptions;
using MerchantApp.Requests;
using MerchantApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserProfileService _userProfileService;


        public UserProfileController(IUserService userService, IUserProfileService userProfileService) 
        {
            _userService = userService;
            _userProfileService = userProfileService;
        }


        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public Object GetUserProfile()
        {
            var username = User.Claims.First(x => x.Type == "Username").Value;
            var user = _userService.MyProfile(username);
            return user;
        }
        [Authorize]
        [HttpPut("EditProfile")]
        public IActionResult EditProfile([FromForm] UserUpdateRequest request)
        {
            try
            {
                var result = _userProfileService.EditProfile(request);
                return Ok(result);
            }
            catch (CustomException e)
            {
                return StatusCode(409, e.Message);
            }
        }
    }
}
