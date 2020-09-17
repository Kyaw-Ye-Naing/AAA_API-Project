using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AAA_API.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using Microsoft.EntityFrameworkCore;

namespace AAA_API.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Gambling_AppContext _context;
       private readonly IConfiguration _configuartion;
        public HomeController(ILogger<HomeController> logger, Gambling_AppContext context,IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuartion = configuration;
        }

        [Route("home")]
        [HttpGet]
        public IEnumerable<TblLeague> Index()
        {
            //For bro aung kyaw nyunt
            //Enable to initialize as a public variable but it can call within classes as shown below
            var connString = _configuartion.GetConnectionString("DefaultConnection");
            return _context.TblLeague.ToList();
        }

        //LogIn 
        [AllowAnonymous]
        [Route("authenicate")]
        [HttpPost]
        public IActionResult LogIn(AuthenicateModel user)
        {
            TblUser tblUser = new TblUser();
            //Check username and password are empty
            if (string.IsNullOrEmpty(user.username) || string.IsNullOrEmpty(user.password)) 
            {
                return BadRequest(new { status = "Usename or password is empty" });
            }

            //Get all information of logged user
             var userValue = _context.TblUser.SingleOrDefault(x => x.Username == user.username && x.Password==user.password && x.Lock==false);

            //Check username and password are incorrect
           if (userValue == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

           //Check logged user account is locked
           var Userlock= _context.TblUser.Where(a => a.Username.Equals(user.username)).FirstOrDefault().Lock;
            if (tblUser.Lock == false)
            {
                return BadRequest(new { message = "Your account is lock!" });
            }

            //return logged user information
            return Ok(userValue);
        }
        

       
    }
}
