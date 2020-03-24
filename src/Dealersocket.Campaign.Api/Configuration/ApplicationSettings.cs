using DealerSocket.AspNetHosting.Mvc.Configuration;

namespace Dealersocket.Campaign.Api.Configuration
{
    public class ApplicationSettings
    {
        public CorsSettings Cors { get; set; }
        public AuthenticationSettings Authentication { get; set; }
        public SwaggerSettings Swagger { get; set; }
    }
}
