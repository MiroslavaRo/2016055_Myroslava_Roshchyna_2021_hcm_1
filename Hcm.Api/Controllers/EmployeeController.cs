using Hcm.Api.Dto;
using Hcm.Api.Services;
using Hcm.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Hcm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeeController(
            EmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = nameof(Roles.Administrator))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<EmployeeDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetListAsync();
            if (result == null 
                || !result.Any())
            {
                return Ok(Enumerable.Empty<EmployeeDto>());
            }

            return Ok(result);
        }

        [HttpGet("{employeeId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EmployeeDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Get(
            [Required] [FromRoute] string employeeId)
        {
            var result = await _service.GetDetailsAsync(employeeId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Administrator))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EmployeeDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Post(
            [FromBody] EmployeeCreateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.CreateAsync(
                request);

            return Ok(result);
        }

        [HttpPut("{employeeId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EmployeeDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Put(
            [Required] [FromRoute] string employeeId, 
            [FromBody] EmployeeUpdateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.UpdateAsync(
                employeeId, request);

            return Ok(result);
        }

        [HttpDelete("{employeeId}")]
        [Authorize(Roles = nameof(Roles.Administrator))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EmployeeDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Delete(
            [Required] [FromRoute] string employeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.DeleteAsync(
                employeeId);

            return Ok(result);
        }
    }
}
