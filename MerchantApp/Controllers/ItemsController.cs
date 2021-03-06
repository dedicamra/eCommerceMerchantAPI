using MerchantApp.Exceptions;
using MerchantApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MerchantApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public readonly IItemService _service;

        public ItemsController(IItemService service)
        {
            _service = service;
        }

        [Authorize(Roles ="Administrator")]
        [HttpPost]
        public IActionResult Insert([FromForm] Requests.ItemInsertRequest request)
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
        
        [Authorize(Roles ="Administrator")]
        [HttpPut("{Id}")]
        public IActionResult Update(int Id, [FromForm] Requests.ItemUpdateRequest request)
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

        [Authorize(Roles ="Administrator, Merchant")]
        [HttpGet]
        public List<Models.Items> GetBySearchTerm([FromQuery] Requests.ItemSearchRequest request)
        {
            return _service.GetBySearchTerm(request);
        }

        [Authorize(Roles ="Administrator, Merchant")]
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

        //[HttpPut("{Id}")]
        //public Models.Items Update(int Id, [FromBody] Requests.ItemInsertRequest request)
        //{
        //    return _service.Update(Id, request);
        //}




        [Authorize(Roles ="Administrator")]
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
