using System;
using System.Web;
using FancyRedirect.Attributes;
using FancyRedirect.DataHandlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UrlShortify.Controllers
{
    [Route("api/reveal")]
    [ApiController]
    public class RevealController : ControllerBase
    {
        /// <summary>
        /// Local logger.
        /// </summary>
        private readonly ILogger Logger;

        /// <summary>
        /// Setup logger.
        /// </summary>
        public RevealController(ILoggerFactory logger)
        {
            this.Logger = logger.CreateLogger("Api.Stats");
        }

        /// <summary>
        /// Reveal a shortened URL.
        /// </summary>
        [HttpGet]
        [RequestRateLimit(Name = "Reveal", Seconds = 5)]
        public ActionResult Reveal([FromQuery] string url)
        {
            url = HttpUtility.UrlDecode(url);

            var code = url.Contains('/')
                ? url.Substring(url.LastIndexOf('/') + 1)
                : url;

            if (string.IsNullOrWhiteSpace(code))
            {
                return this.BadRequest(new
                {
                    message = "Invalid URL!"
                });
            }

            Uri fullUrl = null;

            try
            {
                var entry = StorageHandler.GetByCode(code, false);

                if (entry != null)
                {
                    fullUrl = new Uri(entry.Url);
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, ex.Message);
            }

            if (fullUrl == null)
            {
                return this.NotFound(new
                {
                    message = "URL not found!"
                });
            }

            return this.Ok(new
            {
                url = fullUrl
            });
        }
    }
}