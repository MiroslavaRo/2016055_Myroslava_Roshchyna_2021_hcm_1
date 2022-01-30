using Hcm.Api.Dto;
using Hcm.Api.Services;
using Hcm.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Hcm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly AssignmentService _service;

        public AssignmentController(
            AssignmentService service)
        {
            _service = service;
        }

        [HttpGet("{employeeId}/all")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<AssignmentDto>))]
        [ProducesResponseType((int)HttpStatusCode.NoContent, Type = typeof(IEnumerable<AssignmentDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> GetByEmployee(string employeeId)
        {
            var results = await _service.GetListAsync();
            if (results == null || !results.Any())
            {
                return Ok(Enumerable.Empty<AssignmentDto>());
            }

            return Ok(results
                .Where(e => e.EmployeeId == employeeId)
                .ToArray());
        }

        [HttpGet("{assignmentId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AssignmentDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(object))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Get(
            [FromRoute][Required] string assignmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.GetDetailsAsync(assignmentId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("{employeeId}/{depatmentId}")]
        [Authorize(Roles = nameof(Roles.Administrator))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AssignmentDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Post(
            [Required] [FromRoute] string employeeId,
            [Required] [FromRoute] string depatmentId,
            [FromBody] AssignmentCreateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.CreateAsync(
                employeeId, depatmentId, request);

            return Ok(result);
        }

        [HttpPut("{assignmentId}")]
        [Authorize(Roles = nameof(Roles.Administrator))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AssignmentDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Put(
            [Required] [FromRoute] string assignmentId,
            [FromBody] AssignmentUpdateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.UpdateAsync(
                assignmentId, request);

            return Ok(result);
        }

        [HttpDelete("{assignmentId}")]
        [Authorize(Roles = nameof(Roles.Administrator))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AssignmentDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Delete(
            [Required] [FromRoute] string assignmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.DeleteAsync(
                assignmentId);

            return Ok(result);
        }
    }
}
