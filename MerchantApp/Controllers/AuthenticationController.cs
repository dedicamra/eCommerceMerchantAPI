using Data.EntityModels;
using MerchantApp.Exceptions;
using MerchantApp.Requests;
using MerchantApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        
        [Authorize(Roles="Administrator")]
        [HttpPost("SignUpAdmin")]
        public IActionResult SignUpAdmin([FromForm]AdminInsertRequest request)
        {
            try
            {
                var result = _userService.SignUpAdmin(request);
                return Created("", result);

            }
            catch (CustomException e)
            {
                return StatusCode(409, e.Message);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("SignUpMerchant")]
        public IActionResult SignUpMerchant([FromForm] MerchantInsertRequest request)
        {
            try
            {
                var result = _userService.SignUpMerchant(request);
                return Created("", result);

            }
            catch (CustomException e)
            {
                return StatusCode(409, e.Message);
            }
        }

        [HttpPost("SignIn")]
        public IActionResult SignIn([FromForm]SignInRequest request)
        {
            //return _userService.signIn(username,password);
            try
            {
                var result = _userService.SignIn(request);
                return Ok(result);
            }
            catch (CustomException e)
            {
                return StatusCode(401, e.Message);
            }
        }

       

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            try
            {
                _userService.Delete(Id);
                return Ok();
            }
            catch (CustomException e)
            {
                return StatusCode(404, e.Message);
            }

        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPasswordMail([FromForm] ForgotPasswordRequest request)
        {
            _userService.ForgotPasswordMail(request, Request.Headers["origin"]);
            return Ok(new { message = "Please check your email for password reset instructions" });
        }

        [HttpPost("forgot-password-phone")]
        public IActionResult ForgotPasswordPhone([FromForm] ForgotPasswordPhoneNumberRequest request)
        {
            _userService.ForgotPasswordPhoneNumber(request);
            return Ok(new { message = "Please check your messages for password reset instructions" });
        }
        
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromForm] ResetPasswordRequest request)
        {
            try
            {
                _userService.ResetPassword(request);
                return Ok();
            }
            catch (CustomException e)
            {
                return StatusCode(409, e.Message);
            }
        }
    }
}
