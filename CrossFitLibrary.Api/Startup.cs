using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using CrossFitLibrary.Api.BackgroundServices;
using CrossFitLibrary.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CrossFitLibrary.Api
{
    public class Startup
    {
        private const string AllCors = "All";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(options =>
                options.AddPolicy(AllCors, build => build.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Dev"));

            services.AddHostedService<VideoEditingBackgroundService>();
            services.AddSingleton(_ => Channel.CreateUnbounded<EditVideoChannelMessage>());
            services.AddSingleton<VideoManager>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(AllCors);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}