using Microsoft.AspNetCore.Authentication;
using System.Net;
using System.Net.Http.Headers;

namespace Mango.Services.ShoppingCartAPI.Utility
{
    public class BEApiAuthHandler:DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;
        public BEApiAuthHandler(IHttpContextAccessor httpContextAccessor )
        {
            _accessor = httpContextAccessor ;

        }
        /// <summary>
        /// send token with each reuest outgoing from the application
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
          var token=  await _accessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Call the base class implementation to continue processing the request
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
