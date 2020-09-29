using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AAA_API.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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

        // SHOWING USER LIST : api/TblUsers
        [HttpGet]
        [Authorize(Policy = "Person")]
        public async Task<ActionResult<IEnumerable<TblUser>>> GetTblUser()
        {
            return await _context.TblUser.ToListAsync();
        }

        // SHOWING USER DETAIL : api/TblUsers/5
        [HttpGet("{id}")]
        [Authorize(Policy = "Person")]
        public async Task<ActionResult<TblUser>> GetTblUser(decimal id)
        {
            var tblUser = await _context.TblUser.FindAsync(id);

            if (tblUser == null)
            {
                return NotFound();
            }

            return tblUser;
        }

        // EDITING USER : api/TblUsers/edit/5
        [HttpPut("{id}")]
        [Authorize(Policy = "Person")]
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

        // CREATING USER : api/TblUsers
        [HttpPost]
        [Authorize(Policy = "Person")]
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

        // DELETING USER : api/TblUsers/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Person")]
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

        // ADDING BALANCE : api/TblUsers/add/5
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
            var parent_role = _context.TblUser.Where(a => a.UserId == Decimal.Parse(user_id)).First().RoleId;
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

        // REMOVING BALANCE :  api/TblUsers/remove/5
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
            var parent_role = _context.TblUser.Where(a => a.UserId == Decimal.Parse(user_id)).First().RoleId;
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

        // MANAGING CREDIT BALANCE :  api/TblUsers/credit/5
        [HttpPut("credit/{id}")]
        [Authorize(Policy ="Person")]
        public IActionResult CreditManage(decimal id,Credit creditAmount)
        {
            //Created user's credit
            var user_id = User.FindFirst("userId")?.Value;
            TblCredit credit = new TblCredit();
            DateTime moment = DateTime.Now;
            var role = _context.TblUser.Where(a => a.UserId == id).First().RoleId;
            var year = moment.Year;
            var month = moment.Month;
            var day = moment.Day;
            var hour = moment.Hour;
            var minute = moment.Minute;
            var second = moment.Second;
            var str = "CR" + id.ToString() + role.ToString()+ year.ToString() + month.ToString() + day.ToString() +
                hour.ToString() + minute.ToString() + second.ToString();
            credit.PostingNo = str;
            credit.UserId = id;
            credit.Amount = creditAmount.amount;
            credit.Active = true;
            credit.CreatedBy = Decimal.Parse(user_id);
            credit.CreatedDate = DateTime.Now;
            _context.TblCredit.Add(credit);
            _context.SaveChanges();

            //Current logged user's credit
            TblCredit parent_credit = new TblCredit();
            DateTime parent_moment = DateTime.Now;
            var parent_role = _context.TblUser.Where(a => a.UserId == Decimal.Parse(user_id)).First().RoleId;
            var parent_year = moment.Year;
            var parent_month = moment.Month;
            var parent_day = moment.Day;
            var parent_hour = moment.Hour;
            var parent_minute = moment.Minute;
            var parent_second = moment.Second;
            var parent_str = "CR" + user_id.ToString() + parent_role.ToString() + parent_year.ToString() + parent_month.ToString() + parent_day.ToString() +
                parent_hour.ToString() + parent_minute.ToString() + parent_second.ToString();
            parent_credit.PostingNo = parent_str;
            parent_credit.UserId = Decimal.Parse(user_id);
            parent_credit.Amount = creditAmount.amount;
            parent_credit.Active = true;
            parent_credit.CreatedBy = Decimal.Parse(user_id);
            parent_credit.CreatedDate = DateTime.Now;
            _context.TblCredit.Add(parent_credit);
            _context.SaveChanges();

            //Created user's credit
            TblUserPosting posting = new TblUserPosting();
            posting.PostingNo = str;
            posting.UserId = id;
            posting.Inward= creditAmount.amount;
            posting.Outward = 0;
            posting.Active = true;
            posting.CreatedBy = Decimal.Parse(user_id);
            posting.CreatedDate = DateTime.Now;
            _context.TblUserPosting.Add(posting);
            _context.SaveChanges();

            //Current logged user's credit
            TblUserPosting parent_posting = new TblUserPosting();
            parent_posting.PostingNo = parent_str;
            parent_posting.UserId = Decimal.Parse(user_id);
            parent_posting.Inward = 0;
            parent_posting.Outward = creditAmount.amount;
            parent_posting.Active = true;
            parent_posting.CreatedBy = Decimal.Parse(user_id);
            parent_posting.CreatedDate = DateTime.Now;
            _context.TblUserPosting.Add(parent_posting);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("list")]
        [Authorize(Policy = "Person")]
        public IActionResult UserList()
        {
            var user_id = User.FindFirst("userId")?.Value;
            var id = _context.TblUser.Where(a => a.CreatedBy == Decimal.Parse(user_id)).First().UserId;
            List<TblUserPosting> postings =_context.TblUserPosting.ToList();
            List<ViewUserBalance> result = postings
                 .GroupBy(l => l.UserId)
                 .Select(cl => new ViewUserBalance
                 {
                     UserId =cl.First().UserId,
                     Inward= cl.Sum(c =>c.Inward),
                    Outward = cl.Sum(c =>c.Outward),
                    Balance= cl.Sum(c => c.Inward) - cl.Sum(c => c.Outward),
                 }).ToList();
            return Ok(result.Where(a=>a.UserId==id).ToList());
        }

        private bool TblUserExists(decimal id)
        {
            return _context.TblUser.Any(e => e.UserId == id);
        }
    }
}
