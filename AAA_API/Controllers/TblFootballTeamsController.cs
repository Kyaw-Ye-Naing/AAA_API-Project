using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AAA_API.Models;
using Microsoft.AspNetCore.Authorization;

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

        // SHOWING FOOTBALL TEAMS : api/TblFootballTeams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblFootballTeam>>> GetTblFootballTeam()
        {
            return await _context.TblFootballTeam.ToListAsync();
        }

        // SHOWING FOOTBALL TEAMS DETAIL : api/TblFootballTeams/5
        [HttpGet("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<TblFootballTeam>> GetTblFootballTeam(decimal id)
        {
            var tblFootballTeam = await _context.TblFootballTeam.FindAsync(id);

            if (tblFootballTeam == null)
            {
                return NotFound();
            }

            return tblFootballTeam;
        }

        // EDITING FOOTBALL TEAMS : api/TblFootballTeams/5
        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
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

        private bool TblFootballTeamExists(decimal id)
        {
            return _context.TblFootballTeam.Any(e => e.FootballTeamId == id);
        }
    }
}
