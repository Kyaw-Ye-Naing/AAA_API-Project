using System;
using System.Linq;
using AAA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AAA_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
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
        [Route("home")]
        [Authorize(Policy = Policies.User)] 
        public IActionResult Index()
        {
            //For bro aung kyaw nyunt
            //Enable to initialize as a public variable but it can call within classes as shown below
            var connString = _configuartion.GetConnectionString("DefaultConnection");
            //  return _context.TblLeague.ToList();
            return Ok("This is a response from Admin method");
        }

       //Login 
         [HttpPost]
        [Route("authenicate")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] AuthenicateModel login)
        {
            IActionResult response = Unauthorized();

            //Check username and password are empty
            if (string.IsNullOrEmpty(login.username) || string.IsNullOrEmpty(login.password))
            {
                return BadRequest(new {message= "Usename or password is empty" });
            }

            TblUser user = AuthenticateUser(login);

            //Check logged user account is locked
            var Userlock = _context.TblUser.Where(a => a.Username.Equals(login.username)).FirstOrDefault().Lock;
            if (Userlock == false)
            {
                return BadRequest(new { message = "Your account is lock!" });
            }

            //return logged user information and token
            if (user != null)
            {
                var tokenString = GenerateJWTToken(user);
                response = Ok(new
                {
                    token = tokenString,
                    userDetails = user,
                });
            }
            return response;
        }

        //Validate user information
        TblUser AuthenticateUser(AuthenicateModel loginCredentials)
        {
            TblUser user = _context.TblUser.SingleOrDefault(x => x.Username == loginCredentials.username && x.Password == loginCredentials.password);
            return user;
        }

        //Generate token
        string GenerateJWTToken(TblUser userInfo)
        {
            var role = _context.TblRole.Where(a => a.RoleId == userInfo.RoleId).First().Role;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuartion["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                  // new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                   new Claim("name",userInfo.Username),
                    new Claim("Pwd", userInfo.Password.ToString()),
                    new Claim("role",role),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var token = new JwtSecurityToken(
            issuer: _configuartion["Jwt:Issuer"],
            audience: _configuartion["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(50),
            signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}   
       
       
        
 