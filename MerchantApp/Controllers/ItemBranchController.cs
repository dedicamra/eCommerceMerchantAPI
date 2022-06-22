using MerchantApp.Exceptions;
using MerchantApp.Requests;
using MerchantApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MerchantApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ItemBranchController : ControllerBase
    {
        private readonly IItemBranchService _service;

        public ItemBranchController(IItemBranchService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Merchant")]
        public IActionResult Insert([FromForm] ItemBranchInsertRequest request)
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
        [Authorize(Roles = "Administrator, Merchant")]
        public IActionResult Update(int Id, [FromForm] Requests.ItemBranchInsertRequest request)
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
                return StatusCode(404, e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Merchant")]
        public List<Models.ItemBranch> GetBySearchTerm([FromQuery] Requests.ItemBranchSearchRequest request)
        {
            return _service.GetBySearchTerm(request);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrator, Merchant")]
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
