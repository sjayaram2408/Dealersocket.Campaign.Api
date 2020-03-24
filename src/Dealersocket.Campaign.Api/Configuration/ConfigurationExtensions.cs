using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace Dealersocket.Campaign.Api.Configuration
{
    public static class ConfigurationExtensions
    {
        private const string DefaultConfigSection = "Application";

        public static ApplicationSettings GetApplicationSettings(this IConfiguration config)
        {
            return GetApplicationSettings(config, DefaultConfigSection);
        }

        public static void AddDefaultApplicationSettings(this IServiceCollection collection, ApplicationSettings settings)
        {
            collection.Replace(new ServiceDescriptor(typeof(ApplicationSettings), settings));
        }

        private static ApplicationSettings GetApplicationSettings(this IConfiguration config, string configSection)
        {
            var applicationSettings = config.GetSection(configSection).Get<ApplicationSettings>();

            if (!string.IsNullOrWhiteSpace(applicationSettings.Cors.AllowedOriginsString))
            {
                applicationSettings.Cors.AllowedOrigins =
                    applicationSettings.Cors.AllowedOriginsString
                        ?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            }

            applicationSettings.Swagger.OAuth2Authority = applicationSettings.Authentication.Authority;

            if (string.IsNullOrEmpty(applicationSettings.Swagger.DocumentationLocation))
            {
                var assembly = typeof(Startup).GetTypeInfo().Assembly;
                applicationSettings.Swagger.DocumentationLocation = assembly.Location.Replace(".dll", ".xml");
            }

            return applicationSettings;
        }
    }
}
