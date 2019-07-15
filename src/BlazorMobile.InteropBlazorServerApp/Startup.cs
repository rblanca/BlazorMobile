using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorMobile.InteropBlazorApp.Data;
using BlazorMobile.InteropBlazorApp.Services;
using BlazorMobile.InteropApp.Common.Interfaces;
using BlazorMobile.Common.Services;
using BlazorMobile.Common;
using BlazorMobile.InteropBlazorApp.Services.ServerMock;

namespace BlazorMobile.InteropBlazorApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<IXamarinBridge, XamarinBridgeProxyMock>();
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                var componentEndpointConvention = endpoints.MapBlazorHub();
                componentEndpointConvention.AddComponent(typeof(App), "app");

                endpoints.MapFallbackToFile("index.html");

                BlazorWebViewService.Init(componentEndpointConvention, "blazorXamarin", (bool success) =>
                {
                    Console.WriteLine($"Initialization success: {success}");
                    Console.WriteLine("Device is: " + Device.RuntimePlatform);
                });
            });
        }
    }
}
