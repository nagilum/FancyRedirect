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
            var uri = Storage.GetUrl(code);

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