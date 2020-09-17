using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AAA_API.Models;
using Microsoft.Extensions.Configuration;

namespace AAA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        // GET: api/TblConfirmLeagues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblConfirmLeague>>> GetTblConfirmLeague()
        {
            return await _context.TblConfirmLeague.ToListAsync();
        }

        // GET: api/TblLeagues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblLeague>> GetTblLeague(decimal id)
        {
            var tblLeague = await _context.TblLeague.FindAsync(id);

            if (tblLeague == null)
            {
                return NotFound();
            }

            return tblLeague;
        }

        // PUT: api/TblLeagues/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblLeague(decimal id, TblLeague tblLeague)
        {
            if (id != tblLeague.LeagueId)
            {
                return BadRequest();
            }

            _context.Entry(tblLeague).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblLeagueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { status = "Successfully Updated" });
        }

        // POST: api/TblLeagues
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        [Route("insert")]
        [HttpPost]
        public IActionResult PostTblLeague(TblLeague lg)
        {
            //TblLeague league = _context.TblLeague.Find(lg.id);
            TblConfirmLeague confirmLeague = new TblConfirmLeague()
            {
                LeagueId = lg.LeagueId,
                RapidLeagueId = lg.RapidLeagueId,
                Active = true

            };
            _context.TblConfirmLeague.Add(confirmLeague);
            _context.SaveChangesAsync();
            return Ok(new { status = "Successfully Inserted" });
          // return CreatedAtAction("GetTblConfirmLeague", new { id = tblLeague.LeagueId }, tblLeague);
        }

        //Check id exists
        private bool TblLeagueExists(decimal id)
        {
            return _context.TblLeague.Any(e => e.LeagueId == id);
        }
    }
}
