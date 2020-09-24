using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AAA_API.Models;

namespace AAA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblRolesController : ControllerBase
    {
        private readonly Gambling_AppContext _context;

        public TblRolesController(Gambling_AppContext context)
        {
            _context = context;
        }

        // GET: api/TblRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblRole>>> GetTblRole()
        {
            return await _context.TblRole.ToListAsync();
        }

        // GET: api/TblRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblRole>> GetTblRole(int id)
        {
            var tblRole = await _context.TblRole.FindAsync(id);

            if (tblRole == null)
            {
                return NotFound();
            }

            return tblRole;
        }

        // PUT: api/TblRoles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblRole(int id, TblRole tblRole)
        {
            if (id != tblRole.RoleId)
            {
                return BadRequest();
            }

            _context.Entry(tblRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblRoleExists(id))
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

        // POST: api/TblRoles
        [HttpPost]
        public async Task<ActionResult<TblRole>> PostTblRole(TblRole tblRole)
        {
            TblRole role = new TblRole()
            {
                Role = tblRole.Role,
                Active = true

            };
            _context.TblRole.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblRole", new { id = role.RoleId }, role);
        }

        // DELETE: api/TblRoles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TblRole>> DeleteTblRole(int id)
        {
            var tblRole = await _context.TblRole.FindAsync(id);
            if (tblRole == null)
            {
                return NotFound();
            }

            _context.TblRole.Remove(tblRole);
            await _context.SaveChangesAsync();

            return tblRole;
        }

        private bool TblRoleExists(int id)
        {
            return _context.TblRole.Any(e => e.RoleId == id);
        }
    }
}
