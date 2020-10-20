using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AAA_API.Models;
using AAA_API.Models.Data;
using AAA_API.Models.SP.LogInSP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AAA_API.Controllers.LogIn
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly ILogger<LogInController> _logger;
        private readonly Gambling_AppContext _context;
        private readonly IConfiguration _configuartion;
        public LogInController(ILogger<LogInController> logger, Gambling_AppContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuartion = configuration;
        }

        // USER LOGIN :  api/LogIn/authenticate
        [HttpPost]
        [Route("authenticate")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] AuthenicateModel login)
        {

            //Return logged user information and token
            UserLogin user = AuthenticateUser(login);
            if (user != null)
            {

                //Check logged user account is locked
                var Userlock = _context.TblUser.Where(a => a.Username.Equals(login.Username)).FirstOrDefault().Lock;
                if (Userlock == true)
                {
                    return Ok(new { message = "Your account is locked!" });
                }
                var tokenString = GenerateJWTToken(user);
                return Ok(new
                {
                    token = tokenString,
                    userDetails = user,
                });
            }
            return BadRequest(new { message = "Username or Password is incorrect!" });
        }

        UserLogin AuthenticateUser(AuthenicateModel loginCredentials)
        {
            var response = new UserLogin();
            var connString = _configuartion.GetConnectionString("DefaultConnection");
            SqlConnection sql = new SqlConnection(connString);

            if (sql.State == ConnectionState.Closed)
            {
                sql.Open();
            }
            SqlCommand cmd = new SqlCommand("SP_UserLogin", sql)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@username", loginCredentials.Username);
            cmd.Parameters.AddWithValue("@password", loginCredentials.Password);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.FieldCount != 1)
                {
                    while (reader.Read())
                    {
                        response = MapToLogin(reader);
                    }
                }
                else
                {
                    response = null;
                }
            }
            sql.Close();
            return response;
        }

        //Read data from user login stored procedure
        private UserLogin MapToLogin(SqlDataReader reader)
        {
            return new UserLogin()
            {
                UserId = Convert.ToDecimal(reader["userId"].ToString()),
                Username = reader["username"].ToString(),
                MemberCount = Convert.ToInt32(reader["memberCount"].ToString()),
                Lock = Convert.ToBoolean(reader["lock"].ToString()),
                RoleId = Convert.ToInt32(reader["roleId"].ToString()),
                Role = reader["role"].ToString(),
                Mobile = reader["mobile"].ToString(),
                SharePercent = Convert.ToDecimal(reader["sharePercent"].ToString()),
                BetLimitForMix = Convert.ToDecimal(reader["betLimitForMix"].ToString()),
                BetLimitForSingle = Convert.ToDecimal(reader["betLimitForSingle"].ToString()),
                Inward = Convert.ToDecimal(reader["Inward"].ToString()),
                Outward = Convert.ToDecimal(reader["outward"].ToString()),
                Balance = Convert.ToDecimal(reader["balance"].ToString())
            };
        }

        //Generate token
        string GenerateJWTToken(UserLogin userInfo)
        {
            var role = _context.TblRole.Where(a => a.RoleId == userInfo.RoleId).First().Role;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuartion["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                  // new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                  new Claim("userId",userInfo.UserId.ToString()),
                   new Claim("name",userInfo.Username),
                  //  new Claim("Pwd", userInfo.Password.ToString()),
                    new Claim("roleId",userInfo.RoleId.ToString()),
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
