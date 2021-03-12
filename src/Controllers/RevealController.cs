using System.Web;
using FancyRedirect.DataHandlers;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortify.Controllers
{
    [Route("api/reveal")]
    [ApiController]
    public class RevealController : ControllerBase
    {
        /// <summary>
        /// Reveal a shortened URL.
        /// </summary>
        [HttpGet]
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

            var fullUrl = Storage.GetUrl(code);

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