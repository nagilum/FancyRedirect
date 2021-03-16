using FancyRedirect.DataHandlers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace UrlShortify
{
    public class Program
    {
        /// <summary>
        /// Init all the things..
        /// </summary>
        public static void Main(string[] args)
        {
            // Create the database and table.
            StorageHandler.CreateTable();

            // Init the host.
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(b =>
                {
                    b.UseStartup<Startup>();
                })
                .Build()
                .Run();
        }
    }
}