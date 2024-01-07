using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DDT.Api.Configuration;
using DDT.Api.Filters;
using DDT.Application;
using DDT.Application.Account;
using DDT.Infrastructure;
using Intent.RoslynWeaver.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.AspNetCore.Startup", Version = "1.0")]

namespace DDT.Api
{
    [IntentManaged(Mode.Merge)]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [IntentManaged(Mode.Merge)]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(
                opt => { opt.Filters.Add<ExceptionFilter>(); });

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:5173", "https://ddt-hub.netlify.app")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddApplication(Configuration);
            services.ConfigureApplicationSecurity(Configuration);
            services.ConfigureHealthChecks(Configuration);
            services.ConfigureIdentity();
            services.ConfigureProblemDetails();
            services.ConfigureApiVersioning();
            services.AddInfrastructure(Configuration);
            services.AddRazorPages();
            services.ConfigureSwagger(Configuration);
            services.AddTransient<IAccountEmailSender, AccountEmailSender>();
            // Register SmtpClient with the configuration from appsettings.json
            services.AddTransient<SmtpClient>((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                var smtpConfig = config.GetSection("EmailSettings");
                return new SmtpClient
                {
                    Host = smtpConfig["Host"] ?? string.Empty,
                    Port = int.Parse(smtpConfig["Port"] ?? string.Empty),
                    Credentials = new NetworkCredential(smtpConfig["Username"], smtpConfig["Password"])
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseSerilogRequestLogging();
            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultHealthChecks();
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
            app.UseSwashbuckle(Configuration);
        }
    }
}