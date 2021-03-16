using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FancyRedirect.Attributes;
using FancyRedirect.DataHandlers;

namespace FancyRedirect.Controllers
{
    [Route("api/stats")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        /// <summary>
        /// Get a list of all entries in the database.
        /// </summary>
        [HttpGet]
        [RequestRateLimit(Name = "Stats", Seconds = 5)]
        public ActionResult GetStats()
        {
            List<UrlEntry> list = null;

            try
            {
                list = StorageHandler.GetAll();
            }
            catch
            {
                // TODO
            }

            if (list == null)
            {
                return this.BadRequest(new
                {
                    message = "Unable to get entries from database."
                });
            }

            return this.Ok(list);
        }
    }
}
