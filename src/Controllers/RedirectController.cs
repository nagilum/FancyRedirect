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
        public ActionResult PerformRedirect([FromRoute] string code)
        {
            var url = Storage.GetUrl(code);

            if (url == null)
            {
                return this.NotFound(new
                {
                    message = "URL not found."
                });
            }

            return this.RedirectPermanent(url);
        }
    }
}