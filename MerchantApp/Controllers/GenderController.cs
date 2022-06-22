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
    public class GenderController : ControllerBase
    {
        private readonly IGenderService _service;

        public GenderController(IGenderService service)
        {
            _service = service;
        }

        [Authorize(Roles ="Administrator")]
        [HttpPost]
        public IActionResult Insert([FromForm] Requests.GenderInsertRequest request)
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
        public IActionResult Update(int Id, [FromForm] Requests.GenderInsertRequest request)
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


        [Authorize(Roles ="Administrator, Merchant")]
        [HttpGet]
        public List<Models.Gender> GetBySearchTerm([FromQuery] Requests.SearchRequest request)
        {
            return _service.GetBySearchTerm(request);
        }

        [Authorize(Roles ="Administrator")]
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            if (_service.Delete(Id))
                return Ok();
            return NotFound();

        }
    }
}
