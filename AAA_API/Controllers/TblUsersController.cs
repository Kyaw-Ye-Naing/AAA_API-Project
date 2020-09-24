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

        // PUT: api/TblUsers/5
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

        private bool TblUserExists(decimal id)
        {
            return _context.TblUser.Any(e => e.UserId == id);
        }
    }
}
