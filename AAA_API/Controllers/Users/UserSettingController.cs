using System.Linq;
using AAA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using AAA_API.Models.Data;

namespace AAA_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserSettingController : ControllerBase
    {
        private readonly ILogger<UserSettingController> _logger;
        private readonly Gambling_AppContext _context;
        private readonly IConfiguration _configuartion;
        public UserSettingController(ILogger<UserSettingController> logger, Gambling_AppContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuartion = configuration;
        }

        // LOCKING USER ACCOUNT :  api/UserSetting/lock/5
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
            tblUser.CreatedBy = tblUser.CreatedBy;
            tblUser.CreatedDate = tblUser.CreatedDate;
            _context.SaveChanges();

            return Ok(new { Message = "Successfully locked" });
        }

        // RESETING USER'S PASSWORD FROM MASTER :  api/UserSetting/reset
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
                    user.SharePercent = user.SharePercent;
                    user.CreatedBy = user.CreatedBy;
                    user.CreatedDate = user.CreatedDate;
                    _context.SaveChanges();
                    return Ok(new { Message = "Successfully Changed" });
                }
                return BadRequest(new { Message = "Password is incorrect" });
            }
        }

        // CHANGING USER'S PASSWORD FROM ACCOUNT : api/UserSetting/change 
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
                user.SharePercent = user.SharePercent;
                user.CreatedBy = user.CreatedBy;
                user.CreatedDate = user.CreatedDate;
                _context.SaveChanges();
                return Ok(new { Message = "Successfully Changed" });
            }
            return BadRequest(new { Message = "Password is incorrect" });
        }
    }
}




