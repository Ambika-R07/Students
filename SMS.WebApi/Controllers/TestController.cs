using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace SMS.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Exception")]
        public IActionResult ThrowException()
        {
            _logger.LogError(" error logged via ILogger");
            throw new Exception(" global exception handler.");
        }
    }
}

