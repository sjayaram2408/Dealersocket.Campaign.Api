using Dealersocket.Campaign.Api.Services;
using System.Threading.Tasks;
using Xunit;

namespace Dealersocket.Campaign.Api.Tests.Services
{
    public class ExampleServiceUnitTests
    {
        [Fact]
        public async Task GetExampleTest()
        {
            var service = new ExampleService();
            var actual = await service.GetFooAsync();
            Assert.Equal("Bar", actual);
        }
    }
}
