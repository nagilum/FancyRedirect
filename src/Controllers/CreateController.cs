using System;
using System.Web;
using FancyRedirect.Attributes;
using FancyRedirect.DataHandlers;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortify.Controllers
{
    [Route("api/create")]
    [ApiController]
    public class CreateController : ControllerBase
    {
        /// <summary>
        /// Create a short URL from given URL.
        /// </summary>
        [HttpGet]
        [RequestRateLimit(Name = "Create", Seconds = 5)]
        public ActionResult Create([FromQuery] string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return this.BadRequest(new
                {
                    message = "'url' parameter is required."
                });
            }

            url = HttpUtility.UrlDecode(url);

            Uri uri;

            try
            {
                uri = new Uri(url);
            }
            catch
            {
                return this.BadRequest(new
                {
                    message = "Invalid URL."
                });
            }

            var code = Storage.GetCode(uri);

            if (code == null)
            {
                return this.BadRequest(new
                {
                    message = "Unable to store the URL for an unknown reason."
                });
            }

            return this.Ok(new
            {
                url = $"https://{this.Request.Host}/r/{code}"
            });
        }
    }
}