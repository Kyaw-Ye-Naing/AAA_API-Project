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
        // [Authorize(Policy = Policies.User)]
        [Authorize(Policy = "Person")]
        public IActionResult Index()
        {
            //For bro aung kyaw nyunt
            //Cannot initialize as a public variable but it can call within classes as shown below
            var connString = _configuartion.GetConnectionString("DefaultConnection");
            //  return _context.TblLeague.ToList();
            return Ok("This is a response from Person method");
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
                return BadRequest(new { message = "Usename or password is empty" });
            }

            TblUser user = AuthenticateUser(login);

            //Check logged user account is locked
            var Userlock = _context.TblUser.Where(a => a.Username.Equals(login.username)).FirstOrDefault().Lock;
            if (Userlock == true)
            {
                return BadRequest(new { message = "Your account is locked!" });
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
                  new Claim("userId",userInfo.UserId.ToString()),
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

        //Account lock 
        // PUT: api/Home/5
        [HttpPut("lock/{id}")]
        [Authorize(Policy = "Person")]
        public IActionResult Lock(decimal id)
        {
            TblUser tblUser = _context.TblUser.Find(id);
            tblUser.Username = tblUser.Username;
            tblUser.Password = tblUser.Password;
            tblUser.Lock = true;
            tblUser.RoleId = tblUser.RoleId;
            tblUser.Mobile = tblUser.Mobile;
            tblUser.SharePercent = tblUser.SharePercent;
            tblUser.BetLimitForMix = tblUser.BetLimitForMix;
            tblUser.BetLimitForSingle = tblUser.BetLimitForSingle;
            tblUser.SingleBetCommission5 = tblUser.SingleBetCommission5;
            tblUser.SingleBetCommission8 = tblUser.SingleBetCommission8;
            tblUser.MixBetCommission2count15 = tblUser.MixBetCommission2count15;
            tblUser.MixBetCommission3count20 = tblUser.MixBetCommission3count20;
            tblUser.MixBetCommission4count20 = tblUser.MixBetCommission4count20;
            tblUser.MixBetCommission5count20 = tblUser.MixBetCommission5count20;
            tblUser.MixBetCommission6count20 = tblUser.MixBetCommission6count20;
            tblUser.MixBetCommission7count20 = tblUser.MixBetCommission7count20;
            tblUser.MixBetCommission8count20 = tblUser.MixBetCommission8count20;
            tblUser.MixBetCommission9count25 = tblUser.MixBetCommission9count25;
            tblUser.MixBetCommission10count25 = tblUser.MixBetCommission10count25;
            tblUser.MixBetCommission11count25 = tblUser.MixBetCommission11count25;
            tblUser.CreatedBy = tblUser.CreatedBy;
            tblUser.CreatedDate = tblUser.CreatedDate;
            _context.SaveChanges();

            return Ok(new { Message = "Successfully locked" });
        }

        //Reset Password
        [HttpPost]
        [Route("reset")]
        [Authorize(Policy = "Person")]
        public IActionResult RestPassword(ResetPassword reset)
        {
            if (string.IsNullOrEmpty(reset.NewPassword) || string.IsNullOrEmpty(reset.OldPassword))
            {
                return BadRequest(new { message = "Password is empty" });
            }
            else
            {
                var value = _context.TblUser.ToList().Any(a => a.Password.Equals(reset.OldPassword));
                if (value == true)
                {
                    var userId = _context.TblUser.Where(a => a.Password.Equals(reset.OldPassword)).First().UserId;
                    TblUser user = _context.TblUser.Find(userId);
                    user.Username = user.Username;
                    user.Password = reset.NewPassword;
                    user.Lock = user.Lock;
                    user.RoleId = user.RoleId;
                    user.Mobile = user.Mobile;
                    user.BetLimitForMix = user.BetLimitForMix;
                    user.BetLimitForSingle = user.BetLimitForSingle;
                    user.SingleBetCommission5 = user.SingleBetCommission5;
                    user.SingleBetCommission8 = user.SingleBetCommission8;
                    user.MixBetCommission2count15 = user.MixBetCommission2count15;
                    user.MixBetCommission3count20 = user.MixBetCommission3count20;
                    user.MixBetCommission4count20 = user.MixBetCommission4count20;
                    user.MixBetCommission5count20 = user.MixBetCommission5count20;
                    user.MixBetCommission6count20 = user.MixBetCommission6count20;
                    user.MixBetCommission7count20 = user.MixBetCommission7count20;
                    user.MixBetCommission8count20 = user.MixBetCommission8count20;
                    user.MixBetCommission9count25 = user.MixBetCommission9count25;
                    user.MixBetCommission10count25 = user.MixBetCommission10count25;
                    user.MixBetCommission11count25 = user.MixBetCommission11count25;
                    user.CreatedBy = user.CreatedBy;
                    user.CreatedDate = user.CreatedDate;
                    _context.SaveChanges();
                    return Ok(new { Message = "Successfully Changed" });
                }
                return BadRequest(new { Message = "Password is incorrect" });
            }
        }

        //Change Password 
        [HttpPost]
        [Route("change")]
        public IActionResult ChangePassword(ChangePassword change)
        {
            if (string.IsNullOrEmpty(change.CurrentPassword) || string.IsNullOrEmpty(change.NewPassword) ||
                string.IsNullOrEmpty(change.ConfirmPassword))
            {
                return BadRequest(new { message = "Password is empty" });
            }
            if (change.NewPassword.Equals(change.ConfirmPassword))
            {
                return BadRequest(new { message = "Password is empty" });
            }
            var value = _context.TblUser.ToList().Any(a => a.Password.Equals(change.CurrentPassword));
            if (value == true)
            {
                var userId = _context.TblUser.Where(a => a.Password.Equals(change.CurrentPassword)).First().UserId;
                TblUser user = _context.TblUser.Find(userId);
                user.Username = user.Username;
                user.Password = change.NewPassword;
                user.Lock = user.Lock;
                user.RoleId = user.RoleId;
                user.Mobile = user.Mobile;
                user.BetLimitForMix = user.BetLimitForMix;
                user.BetLimitForSingle = user.BetLimitForSingle;
                user.SingleBetCommission5 = user.SingleBetCommission5;
                user.SingleBetCommission8 = user.SingleBetCommission8;
                user.MixBetCommission2count15 = user.MixBetCommission2count15;
                user.MixBetCommission3count20 = user.MixBetCommission3count20;
                user.MixBetCommission4count20 = user.MixBetCommission4count20;
                user.MixBetCommission5count20 = user.MixBetCommission5count20;
                user.MixBetCommission6count20 = user.MixBetCommission6count20;
                user.MixBetCommission7count20 = user.MixBetCommission7count20;
                user.MixBetCommission8count20 = user.MixBetCommission8count20;
                user.MixBetCommission9count25 = user.MixBetCommission9count25;
                user.MixBetCommission10count25 = user.MixBetCommission10count25;
                user.MixBetCommission11count25 = user.MixBetCommission11count25;
                user.CreatedBy = user.CreatedBy;
                user.CreatedDate = user.CreatedDate;
                _context.SaveChanges();
                return Ok(new { Message = "Successfully Changed" });
            }
            return BadRequest(new { Message = "Password is incorrect" });
        }
    }
}




