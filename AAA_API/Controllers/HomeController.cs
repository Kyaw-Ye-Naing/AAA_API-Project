using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AAA_API.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace AAA_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Gambling_AppContext _context;
        //private IUserService userService;
        public HomeController(ILogger<HomeController> logger, Gambling_AppContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpGet]
        public IEnumerable<TblLeague> Index()
        {
            
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
