using Hcm.Api.Middleware.Helpers;
using Hcm.Core.Database;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Hcm.Api.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ErrorResponseHandler _errorResponseHandler;

        public ExceptionMiddleware(
            IUnitOfWork unitOfWork, 
            ErrorResponseHandler errorResponseHandler)
        {
            _unitOfWork = unitOfWork;
            _errorResponseHandler = errorResponseHandler;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (DomainException ex)
            {
                await _unitOfWork.RollbackAsync();
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                await _errorResponseHandler.WriteResponse(
                    context.Response, ex, false);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await _errorResponseHandler.WriteResponse(
                    context.Response, ex, true);
            }
        }
    }
}
