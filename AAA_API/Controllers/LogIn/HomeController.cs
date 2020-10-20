using AAA_API.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AAA_API.Controllers.LogIn
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Gambling_AppContext _context;
        private readonly IConfiguration _configuartion;
        public HomeController(ILogger<HomeController> logger, Gambling_AppContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuartion = configuration;
        }
        [HttpGet]
        public IActionResult GetAPI()
        {

            return Ok(new
            {
                message = "API can be called successfully"
            }
            );
        }
    }
}
