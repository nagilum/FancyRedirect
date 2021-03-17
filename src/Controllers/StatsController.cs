using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FancyRedirect.Attributes;
using FancyRedirect.DataHandlers;
using Microsoft.Extensions.Logging;

namespace FancyRedirect.Controllers
{
    [Route("api/stats")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        /// <summary>
        /// Local logger.
        /// </summary>
        private readonly ILogger Logger;

        /// <summary>
        /// Setup logger.
        /// </summary>
        public StatsController(ILoggerFactory logger)
        {
            this.Logger = logger.CreateLogger("Api.Stats");
        }

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
            catch (Exception ex)
            {
                this.Logger.LogError(ex, ex.Message);
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
