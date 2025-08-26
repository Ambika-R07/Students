using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace SMS.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
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
