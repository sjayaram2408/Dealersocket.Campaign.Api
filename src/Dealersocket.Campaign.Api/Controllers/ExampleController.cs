using DealerSocket.AspNetHosting.Mvc.Controllers;
using Dealersocket.Campaign.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Dealersocket.Campaign.Api.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ExampleController : BaseController<ExampleController>
    {
        private readonly IExampleService _exampleService;

        public ExampleController(ILogger<ExampleController> logger, IHostingEnvironment environment, IExampleService exampleService)
            : base(logger, environment)
        {
            _exampleService = exampleService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _exampleService.GetFooAsync());
        }
    }
}
