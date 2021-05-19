using FancyRedirect.DataHandlers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

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
            try
            {
                StorageHandler.CreateTable();

                Console.WriteLine($"StoragePath: {StorageHandler.StoragePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("CRITICAL ERROR");
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                Console.WriteLine("ABORTING/EXITING THE APPLICATION!");

                return;
            }

            // Init the host.
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(l =>
                {
                    l.ClearProviders();
                    l.AddConsole();
                })
                .ConfigureWebHostDefaults(b =>
                {
                    b.UseStartup<Startup>();
                })
                .Build()
                .Run();
        }
    }
}