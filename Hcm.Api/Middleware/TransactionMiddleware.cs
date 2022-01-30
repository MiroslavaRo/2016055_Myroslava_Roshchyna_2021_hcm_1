using Hcm.Api.Middleware.Helpers;
using Hcm.Core.Database;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Hcm.Api.Middleware
{
    public class TransactionMiddleware : IMiddleware
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ErrorResponseHandler _errorResponseHandler;

        public TransactionMiddleware(
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
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();

                // Write response to the pipline
                await _errorResponseHandler.WriteResponse(
                    context.Response, ex, !(ex is DomainException));
            }
        }
    }
}
