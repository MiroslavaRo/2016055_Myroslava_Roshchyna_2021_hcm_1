using Hcm.Api.Dto;
using Hcm.Api.Middleware.Helpers;
using Hcm.Api.Services;
using Hcm.Core.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Hcm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly TokenService _service;
        private readonly ErrorResponseHandler _errorResponseHandler;

        public TokenController(
            TokenService service, 
            ErrorResponseHandler errorResponseHandler)
        {
            _service = service;
            _errorResponseHandler = errorResponseHandler;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TokenResultDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ErrorDto))]
        public async Task<IActionResult> Post(
            [FromBody] TokenRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDto(ModelState));
            }

            try
            {
                var result = await _service.IssueTokenAsync(
                    request.Username, request.Password, request.Role);

                return Ok(result);
            }
            catch(DomainException ex)
            {
                return _errorResponseHandler.WriteResponse(ex);
            } 
        }
    }
}
