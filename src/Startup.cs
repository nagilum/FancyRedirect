using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace UrlShortify
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Use((context, func) =>
            {
                context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000";
                context.Response.Headers["Content-Security-Policy"] = "default-src 'none'";
                context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                context.Response.Headers["Referrer-Policy"] = "same-origin";

                return func.Invoke();
            });
        }
    }
}