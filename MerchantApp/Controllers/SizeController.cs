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
    [Authorize(Roles = "Administrator, Merchant")]
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _service;

        public SizeController(ISizeService service)
        {
            _service = service;
        }

        //[Authorize(Roles ="Administrator, Merchant")]
        [HttpPost]
        public IActionResult Insert([FromForm] SizeInsertRequest request)
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

        //[Authorize(Roles ="Administrator, Merchant")]
        [HttpPut("{Id}")]
        public IActionResult Update(int Id, [FromForm] Requests.SizeInsertRequest request)
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

        //[Authorize(Roles ="Administrator, Merchant")]
        [HttpGet]
        public List<Models.Size> GetBySearchTerm([FromQuery] Requests.SizeSearchRequest request)//, bool active=true
        {
            return _service.GetBySearchTerm(request);//,active
        }


        //[Authorize(Roles ="Administrator, Merchant")]
        [HttpGet("{Id}")]
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




        //[Authorize(Roles ="Administrator, Merchant")]
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            try
            {
                var result = _service.Delete(Id);
                return Ok(result);
            }
            catch (System.Exception e)
            {
                return StatusCode(404, e.Message);
            }

        }
    }
}
