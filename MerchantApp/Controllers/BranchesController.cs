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
    public class BranchesController : ControllerBase
    {
        private readonly IBranchesService _service;

        public BranchesController(IBranchesService service)
        {
            _service = service;
        }
     
        [Authorize(Roles ="Administrator")]
        [HttpPost]
        public IActionResult Insert([FromForm] BranchInsertRequest request)
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

        [Authorize(Roles = "Administrator")]
        [HttpPut("{Id}")]
        public IActionResult Update(int Id, [FromForm] Requests.BranchInsertRequest request)
        {
            //if (_service.Update(Id, request) != null)
            //    return Ok();
            //return StatusCode(303, "Request is not valid.");
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

        [Authorize]
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

        [Authorize]
        [HttpGet]
        public List<Models.Branches> GetBySearchTerm([FromQuery] Requests.SearchRequest request)//, bool active=true
        {
            return _service.GetBySearchTerm(request);//,active
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            try
            {
                var result = _service.Delete(Id);
                return Ok(result);
            }
            catch (CustomException e)
            {
                return StatusCode(404, e.Message);
            }

        }


    }
}
