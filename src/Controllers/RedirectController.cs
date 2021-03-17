using System;
using FancyRedirect.Attributes;
using FancyRedirect.DataHandlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UrlShortify.Controllers
{
    [Route("r")]
    [ApiController]
    public class RedirectController : ControllerBase
    {
        /// <summary>
        /// Local logger.
        /// </summary>
        private readonly ILogger Logger;

        /// <summary>
        /// Setup logger.
        /// </summary>
        public RedirectController(ILoggerFactory logger)
        {
            this.Logger = logger.CreateLogger("Api.Stats");
        }

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
            catch (Exception ex)
            {
                this.Logger.LogError(ex, ex.Message);
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