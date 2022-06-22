using MerchantApp.Exceptions;
using MerchantApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MerchantApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCategoryController : ControllerBase
    {
        private readonly IItemCategoryService _service;

        public ItemCategoryController(IItemCategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Insert([FromForm] Requests.ItemCategoryInsertRequest request)
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
        public IActionResult Update(int Id, [FromForm] Requests.ItemCategoryInsertRequest request)
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

        //[HttpGet]
        //public List<Models.ItemCategory> Get()
        //{
        //    return _service.Get();
        //}
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

        //if there is no search term it returns all categories
        [HttpGet]
        [Authorize(Roles = "Administrator, Merchant")]
        public List<Models.ItemCategory> GetBySearchTerm([FromQuery] Requests.SearchRequest request)
        {
            return _service.GetBySearchTerm(request);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrator")]
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
