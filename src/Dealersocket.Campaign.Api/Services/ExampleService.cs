using Dealersocket.Campaign.Api.Services.Interfaces;
using System.Threading.Tasks;

namespace Dealersocket.Campaign.Api.Services
{
    public class ExampleService : IExampleService
    {
        public Task<string> GetFooAsync()
        {
            return Task.FromResult("Bar");
        }
    }
}
