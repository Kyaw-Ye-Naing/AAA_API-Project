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
    public class TblFootballTeamsController : ControllerBase
    {
        private readonly Gambling_AppContext _context;

        public TblFootballTeamsController(Gambling_AppContext context)
        {
            _context = context;
        }

        // GET: api/TblFootballTeams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblFootballTeam>>> GetTblFootballTeam()
        {
            return await _context.TblFootballTeam.ToListAsync();
        }

        // GET: api/TblFootballTeams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblFootballTeam>> GetTblFootballTeam(decimal id)
        {
            var tblFootballTeam = await _context.TblFootballTeam.FindAsync(id);

            if (tblFootballTeam == null)
            {
                return NotFound();
            }

            return tblFootballTeam;
        }

        // PUT: api/TblFootballTeams/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblFootballTeam(decimal id, TblFootballTeam tblFootballTeam)
        {
            if (id != tblFootballTeam.FootballTeamId)
            {
                return BadRequest();
            }

            _context.Entry(tblFootballTeam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblFootballTeamExists(id))
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

        // POST: api/TblFootballTeams
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TblFootballTeam>> PostTblFootballTeam(TblFootballTeam tblFootballTeam)
        {
            _context.TblFootballTeam.Add(tblFootballTeam);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblFootballTeam", new { id = tblFootballTeam.FootballTeamId }, tblFootballTeam);
        }

        // DELETE: api/TblFootballTeams/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TblFootballTeam>> DeleteTblFootballTeam(decimal id)
        {
            var tblFootballTeam = await _context.TblFootballTeam.FindAsync(id);
            if (tblFootballTeam == null)
            {
                return NotFound();
            }

            _context.TblFootballTeam.Remove(tblFootballTeam);
            await _context.SaveChangesAsync();

            return tblFootballTeam;
        }

        private bool TblFootballTeamExists(decimal id)
        {
            return _context.TblFootballTeam.Any(e => e.FootballTeamId == id);
        }
    }
}
