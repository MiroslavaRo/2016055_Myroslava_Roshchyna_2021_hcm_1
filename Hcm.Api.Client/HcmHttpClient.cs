using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Hcm.Api.Client
{
    public class HcmApiHttpClient : HttpClient
    {
        public HcmApiHttpClient(
            IOptions<HcmApiClientSettings> options,
            IHttpContextAccessor httpContextAccessor)
        {
            BaseAddress = new Uri(options.Value.Address);

            if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var value))
            {
                var parts = value.ToString()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                DefaultRequestHeaders.Authorization 
                    = new System.Net.Http.Headers.AuthenticationHeaderValue(parts[0], parts[1]);
            }
        }
    }
}
