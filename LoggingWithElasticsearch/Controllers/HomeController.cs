using Microsoft.AspNetCore.Mvc;

namespace LoggingWithElasticsearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult ErrorLog()
        {
            try
            {
                int a = 1, b = 0;
                var result = a / b;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An {ex.Message} occured in HomeController index action ");
                throw;
            }
            return Ok();
        }
        [HttpGet("/infolog")]
        public IActionResult InfoLog()
        {
            _logger.LogInformation("Info log started");
            return Ok();
        }
    }
}
