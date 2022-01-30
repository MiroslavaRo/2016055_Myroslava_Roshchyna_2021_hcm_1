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
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _service;

        public DepartmentController(
            DepartmentService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<DepartmentDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Get()
        {
            var results = await _service.GetListAsync();
            if (results == null 
                || !results.Any())
            {
                return Ok(Enumerable.Empty<DepartmentDto>());
            }

            return Ok(results);
        }

        [HttpGet("{departmentId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DepartmentDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Get(
            [FromRoute] [Required] string departmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.GetDetailsAsync(departmentId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Administrator))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DepartmentDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Post(
            [FromBody] DepartmentCreateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.CreateAsync(
                request);

            return Ok(result);
        }

        [HttpPut("{departmentId}")]
        [Authorize(Roles = nameof(Roles.Administrator))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DepartmentDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Put(
            [Required] [FromRoute] string departmentId,
            [FromBody] DepartmentUpdateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.UpdateAsync(
                departmentId, request);

            return Ok(result);
        }

        [HttpDelete("{departmentId}")]
        [Authorize(Roles = nameof(Roles.Administrator))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DepartmentDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Delete(
            [Required] [FromRoute] string departmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.DeleteAsync(
                departmentId);

            return Ok(result);
        }
    }
}
