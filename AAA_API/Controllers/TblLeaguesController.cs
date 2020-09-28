using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AAA_API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace AAA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Admin")]
    public class TblLeaguesController : ControllerBase
    {
        private readonly Gambling_AppContext _context;
        private readonly IConfiguration _configuartion;
        public TblLeaguesController(Gambling_AppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuartion = configuration;
        }

        // GET: api/TblLeagues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblLeague>>> GetTblLeague()
        {
            return await _context.TblLeague.ToListAsync();
        }

        // PUT: api/TblLeagues/5
        [HttpPut("{id}")]
        public IActionResult PutTblLeague(decimal id)
        {
            TblLeague league = _context.TblLeague.Find(id);
            if (TblConfirmLeagueExists(league.LeagueId))
            {
                return BadRequest(new { Message = "Selected league is already exist" });
            }
            TblConfirmLeague confirmLeague = new TblConfirmLeague()
            {
                LeagueId = league.LeagueId,
                RapidLeagueId = league.RapidLeagueId,
                Active = true
            };
            _context.TblConfirmLeague.Add(confirmLeague);
            _context.SaveChangesAsync();
            return Ok(new { status = "Successfully Inserted" });
            // return CreatedAtAction("GetTblConfirmLeague", new { id = tblLeague.LeagueId }, tblLeague);
        }

        //Check id exists in league table
        private bool TblLeagueExists(decimal id)
        {
            return _context.TblLeague.Any(e => e.LeagueId == id);
        }

        //Check id exists in Confirm league table
        private bool TblConfirmLeagueExists(decimal id)
        {
            return _context.TblConfirmLeague.Any(e => e.LeagueId== id);
        }
    }
}
