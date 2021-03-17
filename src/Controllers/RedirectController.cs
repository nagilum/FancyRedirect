using System;
using FancyRedirect.Attributes;
using FancyRedirect.DataHandlers;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortify.Controllers
{
    [Route("r")]
    [ApiController]
    public class RedirectController : ControllerBase
    {
        /// <summary>
        /// Redirect the incoming request to a new outgoing URL.
        /// </summary>
        [HttpGet("{code}")]
        [RequestRateLimit(Name = "Redirect", Seconds = 5)]
        public ActionResult PerformRedirect([FromRoute] string code)
        {
            Uri uri = null;

            try
            {
                var entry = StorageHandler.GetByCode(code, true);

                if (entry != null)
                {
                    uri = new Uri(entry.Url);
                }
            }
            catch
            {
                // TODO: Log to console/env.
            }

            if (uri == null)
            {
                return this.NotFound(new
                {
                    message = "URL not found."
                });
            }

            return this.RedirectPermanent(uri.ToString());
        }
    }
}