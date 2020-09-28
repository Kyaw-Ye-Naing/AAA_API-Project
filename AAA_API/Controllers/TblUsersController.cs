using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AAA_API.Models;
using System;
using Microsoft.AspNetCore.Authorization;

namespace AAA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblUsersController : ControllerBase
    {
        private readonly Gambling_AppContext _context;

        public TblUsersController(Gambling_AppContext context)
        {
            _context = context;
        }

        // GET: api/TblUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblUser>>> GetTblUser()
        {
            return await _context.TblUser.ToListAsync();
        }

        // GET: api/TblUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblUser>> GetTblUser(decimal id)
        {
            var tblUser = await _context.TblUser.FindAsync(id);

            if (tblUser == null)
            {
                return NotFound();
            }

            return tblUser;
        }

        // PUT: api/TblUsers/edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblUser(decimal id, TblUser tblUser)
        {
            if (id != tblUser.UserId)
            {
                return BadRequest();
            }

            _context.Entry(tblUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TblUsers
        [HttpPost]
        [Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult<TblUser>> PostTblUser(TblUser tblUser)
        {
            var user_id = User.FindFirst("userId")?.Value;
            TblUser user = new TblUser()
            {
                Username = tblUser.Username,
                Password = tblUser.Password,
                Lock = false,
                RoleId = tblUser.RoleId,
                Mobile = tblUser.Mobile,
                SharePercent = tblUser.SharePercent,
                BetLimitForMix = tblUser.BetLimitForMix,
                BetLimitForSingle = tblUser.BetLimitForSingle,
                SingleBetCommission5 = tblUser.SingleBetCommission5,
                SingleBetCommission8 = tblUser.SingleBetCommission8,
                MixBetCommission2count15 = tblUser.MixBetCommission2count15,
                MixBetCommission3count20 = tblUser.MixBetCommission3count20,
                MixBetCommission4count20 = tblUser.MixBetCommission4count20,
                MixBetCommission5count20 = tblUser.MixBetCommission5count20,
                MixBetCommission6count20 = tblUser.MixBetCommission6count20,
                MixBetCommission7count20 = tblUser.MixBetCommission7count20,
                MixBetCommission8count20 = tblUser.MixBetCommission8count20,
                MixBetCommission9count25 = tblUser.MixBetCommission9count25,
                MixBetCommission10count25 = tblUser.MixBetCommission10count25,
                MixBetCommission11count25 = tblUser.MixBetCommission11count25,
                CreatedBy = Decimal.Parse(user_id),
                CreatedDate = DateTime.Now
            };
            _context.TblUser.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblUser", new { id = user.UserId }, user);
        }

        // DELETE: api/TblUsers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TblUser>> DeleteTblUser(decimal id)
        {
            var tblUser = await _context.TblUser.FindAsync(id);
            if (tblUser == null)
            {
                return NotFound();
            }

            _context.TblUser.Remove(tblUser);
            await _context.SaveChangesAsync();

            return tblUser;
        }

        [HttpPut("add/{id}")]
      [Authorize(Policy = "Person")]
        public IActionResult AddBalance(decimal id,[FromBody]Balance balance)
        {
            //Created user's balance
            var user_id = User.FindFirst("userId")?.Value;
            DateTime moment = DateTime.Now;
            TblUserPosting userPosting = new TblUserPosting();
            var role = _context.TblUser.Where(a => a.UserId == id).First().RoleId;
            var year = moment.Year;
            var month = moment.Month;
            var day = moment.Day;
            var hour = moment.Hour;
            var minute = moment.Minute;
            var second = moment.Second;
            var str = "UB" + id.ToString() + role.ToString() + year.ToString() + month.ToString() + day.ToString() + 
                hour.ToString() + minute.ToString() + second.ToString();
            userPosting.UserId = id;
            userPosting.PostingNo = str;
            userPosting.Inward = balance.amount;
            userPosting.Outward = 0;
            userPosting.Active = true;
            userPosting.CreatedBy = Decimal.Parse(user_id);
            userPosting.CreatedDate = DateTime.Now;
            _context.TblUserPosting.Add(userPosting);
            _context.SaveChanges();

            //Created user' balance
            TblUserBalance userBalance = new TblUserBalance();
            userBalance.PostingNo = str;
            userBalance.UserId = id;
            userBalance.Inward = Convert.ToInt32(balance.amount);
            userBalance.Outward = 0;
            userBalance.CreatedDate = DateTime.Now;
            _context.TblUserBalance.Add(userBalance);
            _context.SaveChanges();

            //Current logged user's balance
            DateTime parent_moment =DateTime.Now;
            TblUserPosting parent_userPosting = new TblUserPosting();
            var parent_role = _context.TblUser.Where(a => a.UserId == id).First().RoleId;
            var parent_year = parent_moment.Year;
            var parent_month = parent_moment.Month;
            var parent_day = parent_moment.Day;
            var parent_hour = parent_moment.Hour;
            var parent_minute = parent_moment.Minute;
            var parent_second = parent_moment.Second;
            var parent_str = "UB" + id.ToString() + parent_role.ToString() + parent_year.ToString() + parent_month.ToString() + parent_day.ToString() +
                parent_hour.ToString() + parent_minute.ToString() + parent_second.ToString();
            parent_userPosting.UserId = Decimal.Parse(user_id);
            parent_userPosting.PostingNo = parent_str;
            parent_userPosting.Inward = 0;
            parent_userPosting.Outward = balance.amount;
            parent_userPosting.Active = true;
            parent_userPosting.CreatedBy = Decimal.Parse(user_id);
            parent_userPosting.CreatedDate = DateTime.Now;
            _context.TblUserPosting.Add(parent_userPosting);
            _context.SaveChanges();

            //Current logged user's balance
            TblUserBalance parent_userBalance = new TblUserBalance();
            parent_userBalance.PostingNo = parent_str;
            parent_userBalance.UserId = Decimal.Parse(user_id);
            parent_userBalance.Inward = 0;
            parent_userBalance.Outward = Convert.ToInt32(balance.amount);
            parent_userBalance.CreatedDate = DateTime.Now;
            _context.TblUserBalance.Add(parent_userBalance);
            _context.SaveChanges();
            return Ok(new { message = "Deposit Successfully" });
        }

        [HttpPut("remove/{id}")]
        [Authorize(Policy = "Person")]
        public IActionResult RemoveBalance(decimal id, [FromBody] Balance balance)
        {
            //Created user's balance
            var user_id = User.FindFirst("userId")?.Value;
            DateTime moment =DateTime.Now;
            TblUserPosting userPosting = new TblUserPosting();
            var role = _context.TblUser.Where(a => a.UserId == id).First().RoleId;
            var year = moment.Year;
            var month = moment.Month;
            var day = moment.Day;
            var hour = moment.Hour;
            var minute = moment.Minute;
            var second = moment.Second;
            var str = "UB" + id.ToString() + role.ToString() + year.ToString() + month.ToString() + day.ToString() +
                hour.ToString() + minute.ToString() + second.ToString();
            userPosting.UserId = id;
            userPosting.PostingNo = str;
            userPosting.Inward = 0;
            userPosting.Outward = balance.amount;
            userPosting.Active = true;
            userPosting.CreatedBy = Decimal.Parse(user_id);
            userPosting.CreatedDate = DateTime.Now;
            _context.TblUserPosting.Add(userPosting);
            _context.SaveChanges();

            //Created user' balance
            TblUserBalance userBalance = new TblUserBalance();
            userBalance.PostingNo = str;
            userBalance.UserId = id;
            userBalance.Inward = 0;
            userBalance.Outward = Convert.ToInt32(balance.amount);
            userBalance.CreatedDate = DateTime.Now;
            _context.TblUserBalance.Add(userBalance);
            _context.SaveChanges();

            //Current logged user's balance
            DateTime parent_moment =DateTime.Now;
            TblUserPosting parent_userPosting = new TblUserPosting();
            var parent_role = _context.TblUser.Where(a => a.UserId == id).First().RoleId;
            var parent_year = parent_moment.Year;
            var parent_month = parent_moment.Month;
            var parent_day = parent_moment.Day;
            var parent_hour = parent_moment.Hour;
            var parent_minute = parent_moment.Minute;
            var parent_second = parent_moment.Second;
            var parent_str = "UB" + id.ToString() + parent_role.ToString() + parent_year.ToString() + parent_month.ToString() + parent_day.ToString() +
                parent_hour.ToString() + parent_minute.ToString() + parent_second.ToString();
            parent_userPosting.UserId = Decimal.Parse(user_id);
            parent_userPosting.PostingNo = parent_str;
            parent_userPosting.Inward = balance.amount;
            parent_userPosting.Outward = 0;
            parent_userPosting.Active = true;
            parent_userPosting.CreatedBy = Decimal.Parse(user_id);
            parent_userPosting.CreatedDate = DateTime.Now;
            _context.TblUserPosting.Add(parent_userPosting);
            _context.SaveChanges();

            //Current logged user's balance
            TblUserBalance parent_userBalance = new TblUserBalance();
            parent_userBalance.PostingNo = parent_str;
            parent_userBalance.UserId = Decimal.Parse(user_id);
            parent_userBalance.Inward = Convert.ToInt32(balance.amount);
            parent_userBalance.Outward =0 ;
            parent_userBalance.CreatedDate = DateTime.Now;
            _context.TblUserBalance.Add(parent_userBalance);
            _context.SaveChanges();
            return Ok(new { message = "Withdraw Successfully " });
        }

        private bool TblUserExists(decimal id)
        {
            return _context.TblUser.Any(e => e.UserId == id);
        }
    }
}
