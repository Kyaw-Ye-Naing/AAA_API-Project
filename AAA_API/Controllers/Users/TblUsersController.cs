using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AAA_API.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using AAA_API.Models.Data;

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
                    userCommission.SubUserCommission = s.SubUserCommission;
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

        // SHOWING USER LIST CREATED BY LOGGED USER : api/TblUsers/list
        [HttpGet]
        [Route("list")]
        [Authorize(Policy = "Person")]
        public IActionResult UserList()
        {
            var user_id = User.FindFirst("userId")?.Value;
            var id = _context.TblUser.Where(a => a.CreatedBy == Decimal.Parse(user_id)).First().UserId;
            List<TblUserPosting> postings = _context.TblUserPosting.ToList();
            List<ViewUserBalance1> result = postings
                 .GroupBy(l => l.UserId)
                 .Select(cl => new ViewUserBalance1
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
