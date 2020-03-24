using Dealersocket.Campaign.Api.Controllers;
using Dealersocket.Campaign.Api.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Dealersocket.Campaign.Api.Tests.Controllers
{
    public class ClientsControllerUnitTests
    {
        private readonly Mock<ILogger<ExampleController>> _logger;
        private readonly Mock<IExampleService> _exampleService;
        private readonly Mock<IHostingEnvironment> _environment;

        public ClientsControllerUnitTests()
        {
            _logger = new Mock<ILogger<ExampleController>>();
            _exampleService = new Mock<IExampleService>();
            _environment = new Mock<IHostingEnvironment>();
        }

        [Fact]
        public async Task GetNonExistentClientTest()
        {
            _exampleService.Setup(d => d.GetFooAsync()).ReturnsAsync("Baz");

            var controller = GetClientsController(_logger.Object, _environment.Object, _exampleService.Object);

            var actionResult = await controller.Get();
            _exampleService.Verify(a => a.GetFooAsync(), Times.Once);
            Assert.IsType<OkObjectResult>(actionResult);
        }

        private static ExampleController GetClientsController(ILogger<ExampleController> logger, IHostingEnvironment environment, IExampleService clientsService)
        {
            var clientsController = new Mock<ExampleController>(logger, environment, clientsService) { CallBase = true };
            return clientsController.Object;
        }
    }
}
