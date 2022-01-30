using Hcm.Api.Dto;
using Hcm.Api.Services;
using Hcm.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(
            UserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = nameof(Roles.Administrator))]
        public async Task<IActionResult> Get()
        {
            var results = await _service.GetListAsync();
            if (results == null || !results.Any())
            {
                return Ok(Enumerable.Empty<UserDto>());
            }

            return Ok(results);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(
            [FromRoute] [Required] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.GetDetailsAsync(userId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Administrator))]
        public async Task<IActionResult> Post(
            [FromBody] CreateAdministratorDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.CreateAdministratorAsync(
                request);

            return Ok(result);
        }

        [HttpPut("{userId}")]
        [Authorize(Roles = nameof(Roles.Administrator))]
        public async Task<IActionResult> Put(
            [Required] [FromRoute] string userId,
            [FromBody] UpdateAdministratorDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.UpdateAdministratorAsync(
                userId, request);

            return Ok(result);
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = nameof(Roles.Administrator))]
        public async Task<IActionResult> Delete(
            [Required][FromRoute] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            var result = await _service.DeleteAdministratorAsync(
                userId);

            return Ok(result);
        }

        [HttpPut("password/change/{userId}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(
            [Required] string userId,
            [FromBody] ChangePasswordDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            await _service.ChangePasswordAsync(
                userId, request.OldPassword, request.NewPassword);

            return Ok();
        }
    }
}
