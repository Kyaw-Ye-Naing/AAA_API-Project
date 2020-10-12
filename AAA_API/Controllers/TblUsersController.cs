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
        public IActionResult PutTblUser(decimal id, Commission tblUser)
        {
            if (id != tblUser.UserId)
            {
                return BadRequest();
            }
            var userRole = User.FindFirst("roleId")?.Value;
            var userId = User.FindFirst("userId")?.Value;
            if (!TblUserExists(id))
            {
                return NotFound();
            }
            else
            {
                //Update user information
                //Admin
                if (Convert.ToInt32(userRole) == 1)
                {
                    TblUser editUser = _context.TblUser.Find(id);
                    editUser.Username = tblUser.Username;
                    editUser.Mobile = tblUser.Mobile;
                    editUser.Password = tblUser.Password;
                    editUser.RoleId = tblUser.RoleId;
                    editUser.Lock = false;
                    editUser.SharePercent = tblUser.SharePercent;
                    editUser.BetLimitForMix = tblUser.BetLimitForMix;
                    editUser.CreatedBy = Decimal.Parse(userId);
                    editUser.CreatedDate = DateTime.Now;
                    _context.SaveChanges();

                    //Find commissions of current logged user and its created user
                    var tblUser1 = _context.TblUserCommission.Where(a => a.UserId == Decimal.Parse(userId) && a.SubUserId == id)
                                                            .Select(a => a.UserCommissionId)
                                                            .ToList();

                    //Delete previous user commissions
                    foreach (var item in tblUser1)
                    {
                        TblUserCommission editUser1 = _context.TblUserCommission.Find(item);
                        _context.TblUserCommission.Remove(editUser1);
                        _context.SaveChanges();
                    }

                    //Created new user commissions
                    foreach (var s in tblUser.commission)
                    {
                        TblUserCommission userCommission = new TblUserCommission();
                        userCommission.UserCommissionTypeId = s.CommissionTypeId;
                        userCommission.UserId = Decimal.Parse(userId);
                        userCommission.UserCommission = 0;
                        userCommission.SubUserId = id;
                        userCommission.SubUserCommission = s.SubUserCommission;
                        _context.TblUserCommission.Add(userCommission);
                    }
                    _context.SaveChanges();
                }//admin

                //Other user(seniorMaster,Master,Agent)
                else
                {
                    TblUser editUser = _context.TblUser.Find(id);
                    editUser.Username = tblUser.Username;
                    editUser.Mobile = tblUser.Mobile;
                    editUser.Password = tblUser.Password;
                    editUser.RoleId = tblUser.RoleId;
                    editUser.Lock = false;
                    editUser.SharePercent = tblUser.SharePercent;
                    editUser.BetLimitForMix = tblUser.BetLimitForMix;
                    editUser.CreatedBy = Decimal.Parse(userId);
                    editUser.CreatedDate = DateTime.Now;
                    _context.SaveChanges();

                    //Find commissions of current logged user and its created user
                    var tblUser1 = _context.TblUserCommission.Where(a => a.UserId == Decimal.Parse(userId) && a.SubUserId == id)
                                                            .Select(a => a.UserCommissionId)
                                                            .ToList();

                    //Delete previous user commissions
                    foreach (var item in tblUser1)
                    {
                        TblUserCommission editUser1 = _context.TblUserCommission.Find(item);
                        _context.TblUserCommission.Remove(editUser1);
                        _context.SaveChanges();
                    }

                    //Get previous user commissions
                    var prevCommission = _context.TblUserCommission.Where(a => a.SubUserId == Decimal.Parse(userId))
                                                             .Select(c => new { c.UserCommissionTypeId, c.SubUserCommission })
                                                             .ToDictionary(c => c.UserCommissionTypeId, c => c.SubUserCommission);

                    //Create new user commissions
                    foreach (var s in tblUser.commission)
                    {
                        TblUserCommission userCommission = new TblUserCommission();
                        userCommission.UserCommissionTypeId = s.CommissionTypeId;
                        userCommission.UserId = Decimal.Parse(userId);
                        userCommission.UserCommission = prevCommission.ElementAt(1).Value - s.SubUserCommission;
                        userCommission.SubUserId = id;
                        userCommission.SubUserCommission = s.SubUserCommission;
                        _context.TblUserCommission.Add(userCommission);
                    }
                    _context.SaveChanges();
                }//other

            }//user id exists
            return Ok();
        }


        // CREATING USER : api/TblUsers
        [HttpPost]
        [Authorize(Policy = "Person")]
        public IActionResult PostTblUser(Commission tblUser)
        {

            var userRole = User.FindFirst("roleId")?.Value;
            var userId = User.FindFirst("userId")?.Value;

            //Admin
            if (Convert.ToInt32(userRole) == 1)
            {
                //Create user information
                TblUser user = new TblUser()
                {
                    Username = tblUser.Username,
                    Mobile = tblUser.Mobile,
                    Password = tblUser.Password,
                    RoleId = tblUser.RoleId,
                    Lock = false,
                    SharePercent = tblUser.SharePercent,
                    BetLimitForMix = tblUser.BetLimitForMix,
                    CreatedBy = Decimal.Parse(userId),
                    CreatedDate = DateTime.Now
                };
                _context.TblUser.Add(user);
                _context.SaveChanges();
                var id = user.UserId;

                //Create user's commission
                foreach (var s in tblUser.commission)
                {
                    TblUserCommission userCommission = new TblUserCommission();
                    userCommission.UserCommissionTypeId = s.CommissionTypeId;
                    userCommission.UserId = Decimal.Parse(userId);
                    userCommission.UserCommission = 0;
                    userCommission.SubUserId = id;
                    userCommission.SubUserCommission = s.SubUserCommission;
                    _context.TblUserCommission.Add(userCommission);
                }
                _context.SaveChanges();
            }//Admin

            //Other(SeniorMaster,Master,Agent)
            else
            {
                //Create user information
                TblUser user = new TblUser()
                {
                    Username = tblUser.Username,
                    Mobile = tblUser.Mobile,
                    Password = tblUser.Password,
                    RoleId = tblUser.RoleId,
                    Lock = false,
                    SharePercent = tblUser.SharePercent,
                    BetLimitForMix = tblUser.BetLimitForMix,
                    CreatedBy = Decimal.Parse(userId),
                    CreatedDate = DateTime.Now
                };
                _context.TblUser.Add(user);
                _context.SaveChanges();
                var id = user.UserId;



                var prevCommission = _context.TblUserCommission.Where(a => a.SubUserId == Decimal.Parse(userId))
                                                             .Select(c => new { c.UserCommissionTypeId, c.SubUserCommission })
                                                              .ToDictionary(c => c.UserCommissionTypeId, c => c.SubUserCommission);

                //Create user's commission
                foreach (var s in tblUser.commission)
                {
                    TblUserCommission userCommission = new TblUserCommission();
                    userCommission.UserCommissionTypeId = s.CommissionTypeId;
                    userCommission.UserId = Decimal.Parse(userId);
                    userCommission.UserCommission = prevCommission.ElementAt(1).Value - s.SubUserCommission;
                    userCommission.SubUserId = id;
                    userCommission.SubUserCommission =s.SubUserCommission;
                    _context.TblUserCommission.Add(userCommission);
                }
                _context.SaveChanges();
            }//Other
           return Ok();
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
        public IActionResult AddBalance(decimal id, [FromBody] Balance balance)
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
            userPosting.Inward = balance.Amount;
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
            userBalance.Inward = Convert.ToInt32(balance.Amount);
            userBalance.Outward = 0;
            userBalance.CreatedDate = DateTime.Now;
            _context.TblUserBalance.Add(userBalance);
            _context.SaveChanges();

            //Current logged user's balance
            DateTime parent_moment = DateTime.Now;
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
            parent_userPosting.Outward = balance.Amount;
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
            parent_userBalance.Outward = Convert.ToInt32(balance.Amount);
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
            userPosting.Inward = 0;
            userPosting.Outward = balance.Amount;
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
            userBalance.Outward = Convert.ToInt32(balance.Amount);
            userBalance.CreatedDate = DateTime.Now;
            _context.TblUserBalance.Add(userBalance);
            _context.SaveChanges();

            //Current logged user's balance
            DateTime parent_moment = DateTime.Now;
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
            parent_userPosting.Inward = balance.Amount;
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
            parent_userBalance.Inward = Convert.ToInt32(balance.Amount);
            parent_userBalance.Outward = 0;
            parent_userBalance.CreatedDate = DateTime.Now;
            _context.TblUserBalance.Add(parent_userBalance);
            _context.SaveChanges();
            return Ok(new { message = "Withdraw Successfully " });
        }

        // MANAGING CREDIT BALANCE :  api/TblUsers/credit/5
        [HttpPut("credit/{id}")]
        [Authorize(Policy = "Person")]
        public IActionResult CreditManage(decimal id, Credit creditAmount)
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
            var str = "CR" + id.ToString() + role.ToString() + year.ToString() + month.ToString() + day.ToString() +
                hour.ToString() + minute.ToString() + second.ToString();
            credit.PostingNo = str;
            credit.UserId = id;
            credit.Amount = creditAmount.Amount;
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
            parent_credit.Amount = creditAmount.Amount;
            parent_credit.Active = true;
            parent_credit.CreatedBy = Decimal.Parse(user_id);
            parent_credit.CreatedDate = DateTime.Now;
            _context.TblCredit.Add(parent_credit);
            _context.SaveChanges();

            //Created user's credit
            TblUserPosting posting = new TblUserPosting();
            posting.PostingNo = str;
            posting.UserId = id;
            posting.Inward = creditAmount.Amount;
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
            parent_posting.Outward = creditAmount.Amount;
            parent_posting.Active = true;
            parent_posting.CreatedBy = Decimal.Parse(user_id);
            parent_posting.CreatedDate = DateTime.Now;
            _context.TblUserPosting.Add(parent_posting);
            _context.SaveChanges();
            return Ok();
        }

        // SHOWING USER LIST CREATED BY LOGGED USER
        [HttpGet]
        [Route("list")]
        [Authorize(Policy = "Person")]
        public IActionResult UserList()
        {
            var user_id = User.FindFirst("userId")?.Value;
            var id = _context.TblUser.Where(a => a.CreatedBy == Decimal.Parse(user_id)).First().UserId;
            List<TblUserPosting> postings = _context.TblUserPosting.ToList();
            List<ViewUserBalance> result = postings
                 .GroupBy(l => l.UserId)
                 .Select(cl => new ViewUserBalance
                 {
                     UserId = cl.First().UserId,
                     Inward = cl.Sum(c => c.Inward),
                     Outward = cl.Sum(c => c.Outward),
                     Balance = cl.Sum(c => c.Inward) - cl.Sum(c => c.Outward),
                 }).ToList();
            return Ok(result.Where(a => a.UserId == id).ToList());
        }

        private bool TblUserExists(decimal id)
        {
            return _context.TblUser.Any(e => e.UserId == id);
        }
    }
}
