using AUTHENTICATIONService.Models;
using AUTHENTICATIONService.Models.Wraps;
using AUTHENTICATIONService.Services;
using AUTHENTICATIONService.Services.Abstractions;
using AUTHENTICATIONService.Services.Implementations;
using AUTHENTICATIONService.Services.TokenValidators;
using Config;
using DataBaseStaff;
using DataBaseStaff.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTHENTICATIONService
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

       

        public void ConfigureServices(IServiceCollection services)
        {

            AuthenticationConfig authConfig = new AuthenticationConfig();
            Configuration.Bind("Authentication", authConfig);
            services.AddSingleton(authConfig);

            services.AddControllers();

            services.AddSingleton<EfDbContext>();
            services.AddSingleton<HashTagService>();
            services.AddSingleton<BllLayer>();

            services.AddSingleton<GenerationToken>();
            services.AddSingleton<RefreshTokenValidator>();
            services.AddSingleton<AccessTokenGenerator>();
            services.AddSingleton<AccessTokenRefresh>();
            services.AddSingleton<AuthenticateService>();

            //services.AddMemoryCache();

            services.AddSingleton<IPasswordHasher, PasswordHasher>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
