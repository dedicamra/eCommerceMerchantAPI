using MerchantApp.Exceptions;
using MerchantApp.Models;
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
    public class ItemDetailsController : ControllerBase
    {
        private readonly IItemDetailsService _service;

        public ItemDetailsController(IItemDetailsService service)
        {
            _service = service;
        }


        [HttpPost]
        [Authorize(Roles = "Merchant")]
        public IActionResult Insert([FromForm] Requests.ItemDetailsInsertRequest request)
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
        [Authorize(Roles = "Merchant")]
        public IActionResult Update(int Id, [FromForm] Requests.ItemDetailsInsertRequest request)
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
        public List<ItemDetails> GetBySearchTerm([FromQuery] Requests.ItemDetailsSearchRequest request)
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
        [Authorize(Roles = "Merchant")]
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
