using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace FancyRedirect.Attributes
{
    public class RequestRateLimitAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Name of the filter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// How many seconds between requests.
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Memory cache to use.
        /// </summary>
        public static MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// Check the request and terminate if the same IP is requesting too often.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ip = context.HttpContext.Request.HttpContext.Connection.RemoteIpAddress;
            var key = $"{this.Name}-{ip}";

            if (!Cache.TryGetValue(key, out var _))
            {
                var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(this.Seconds));

                Cache.Set(
                    key,
                    true,
                    options);
            }
            else
            {
                context.Result = new ContentResult
                {
                    Content = "{\"message\":\"Requests are limited to 1, every " + this.Seconds + " seconds.\"}"
                };

                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            }
        }
    }
}