using DealerSocket.AspNetHosting.Mvc.Extensions;
using DealerSocket.AspNetHosting.Mvc.Filters;
using DealerSocket.AspNetHosting.Mvc.Logging;
using Dealersocket.Campaign.Api.Configuration;
using Dealersocket.Campaign.Api.Services;
using Dealersocket.Campaign.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dealersocket.Campaign.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
            Environment = env;

            loggerFactory.AddLambdaLogger(Configuration.GetLambdaLoggerOptions());

            Logger.LogInformation("Startup for {EnvironmentName}.", env.EnvironmentName);

            Audit.Core.Configuration.DataProvider = new AuditLogCloudWatchProvider();
        }

        public static ApplicationSettings Settings { get; set; }
        public IConfiguration Configuration { get; }
        private ILogger<Startup> Logger { get; }
        private IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            Logger.LogInformation("Add settings from configuration to dependency injection framework");
            var settings = Settings ?? Configuration.GetApplicationSettings();
            Logger.LogInformation("Settings: {@Settings}", settings);

            services.AddDefaultApplicationSettings(settings);

            services.AddAuthenticationService(settings.Authentication);

            Logger.LogInformation("Add controllers and api services to dependency injection framework");
            services.AddMvc(op =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                op.Filters.Add(new AuthorizeFilter(policy));
                op.Filters.Add(new GlobalExceptionFilter(Environment));
                op.Filters.Add(new GlobalValidateModelStateFilter());
            });

            services.AddSwaggerService(settings.Swagger);

            Logger.LogInformation("Add application services to the dependency injection framework");
            services.AddTransient<IExampleService, ExampleService>();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, ApplicationSettings settings)
        {
            Logger.LogInformation("Set up pipeline");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor
            });
            app.UseRequestLogging();
            app.UseDefaultCorsPolicy(settings.Cors);
            app.UseAuthentication();
            app.UseMvc();
            if (Environment.IsProduction())
            {
                return;
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dealersocket.Campaign.Api");
                c.OAuthClientId("crm_campaign_api");
            });
        }
    }
}
