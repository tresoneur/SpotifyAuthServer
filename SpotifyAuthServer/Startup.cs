using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SpotifyAuthServer.Business;
using SpotifyAuthServer.Data.Context;
using SpotifyAuthServer.Data.Repository;
using SpotifyAuthServer.Data.Repository.Interfaces;
using SpotifyAuthServer.Services;

namespace SpotifyAuthServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => 
                    builder
                        .WithOrigins(Configuration["AppUri"])
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddControllers();

            services.AddDbContext<UserDbContext>(options => options.UseSqlite($"Data Source=auth.db"));

            services.AddScoped<UserDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<SpotifyAuthService>();
            services.AddScoped<UserManager>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
