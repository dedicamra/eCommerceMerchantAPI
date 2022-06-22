using MerchantApp.Exceptions;
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
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _service;

        public CouponsController(ICouponService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Insert([FromForm] Requests.CouponInsertRequest request)
        {
            try
            {
                var result = _service.Insert(request);
                return Created("", result);
            }
            catch (CustomException e)
            {
                return StatusCode(409, e.Message);
            }
        }

        [HttpPut("{Id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Update(int Id, [FromForm] Requests.CouponInsertRequest request)
        {
            try
            {
                var result = _service.Update(Id, request);
                return Ok(result);
            }
            catch (CustomException e)
            {
                return StatusCode(409, e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Merchant")]
        public List<Models.Coupons> GetBySearchTerm([FromQuery] Requests.CouponSearchRequest request)
        {
            return _service.GetBySearchTerm(request);
        }

        [HttpGet("{Id}")]
        [Authorize(Roles = "Administrator, Merchant")]
        public IActionResult GetById(int Id)
        {
            try
            {
                var result = _service.GetById(Id);
                return Ok(result);

            }
            catch (CustomException e)
            {
                return StatusCode(404, e.Message); ;
            }
        }

        [HttpDelete("{Id}")] 
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int Id)
        {
            try
            {
                var result = _service.Delete(Id);
                return Ok();
            }
            catch (CustomException e)
            {
                return StatusCode(404, e.Message);
            }
        }
    }
}
