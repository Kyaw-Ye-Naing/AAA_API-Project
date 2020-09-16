using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AAA_API.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace AAA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Gambling_AppContext db;
        public HomeController(ILogger<HomeController> logger, Gambling_AppContext _db)
        {
            _logger = logger;
            db = _db;
        }


        [HttpGet]
        public IEnumerable<TblLeague> Index()
        {
            
            return db.TblLeague.ToList();
        }

        

       
    }
}
