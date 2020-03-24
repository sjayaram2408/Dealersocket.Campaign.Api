using Amazon.Lambda.AspNetCoreServer;
using DealerSocket.AspNetHosting.Mvc.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.IO;

namespace Dealersocket.Campaign.Api
{
    public class LambdaEntryPoint : APIGatewayProxyFunction
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("app.settings.json", false, true)
            .AddEnvironmentVariables()
            .Build();

        protected override void Init(IWebHostBuilder builder)
        {
            Configuration.ConfigureLambdaLogger();

            builder
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseConfiguration(Configuration)
                .UseSerilog()
                .UseLambdaServer();
        }
    }
}
