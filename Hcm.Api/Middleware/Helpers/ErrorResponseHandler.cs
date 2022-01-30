using Hcm.Api.Dto;
using Hcm.Core.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Hcm.Api.Middleware.Helpers
{
    public class ErrorResponseHandler
    {
        public async Task WriteResponse(
            HttpResponse response, 
            Exception ex, 
            bool isUnhandled)
        {
            var content = JsonConvert.SerializeObject(new ErrorDto
            {
                Message = ex.Message,
                Error = true,
                Unhandled = isUnhandled,
            });

            response.ContentType = Application.Json;
            response.ContentLength = Encoding.UTF8.GetByteCount(content);

            await response.BodyWriter
                .WriteAsync(Encoding.UTF8.GetBytes(content));
        }

        public IActionResult WriteResponse(
            DomainException ex)
        {
            return new UnauthorizedObjectResult(new ErrorDto
            {
                Message = ex.Message,
                Error = true,
                Unhandled = false,
            });
        }
    }
}
