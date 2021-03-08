﻿using System.Web;
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

            var code = Storage.GetCode(url);

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