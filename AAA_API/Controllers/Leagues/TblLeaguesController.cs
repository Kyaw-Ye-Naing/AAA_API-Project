using AAA_API.Models;
using AAA_API.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // SHOWING LEAGUES : api/TblLeagues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblLeague>>> GetTblLeague()
        {
            return await _context.TblLeague.ToListAsync();
        }

        // INSERTING LEAGUES INTO CONFIRM LEAGUES : api/TblLeagues
        [HttpPost]
        [System.Obsolete]
        public IActionResult PostTblLeague(ConfirmLeague confrimLeague)
        {
            _context.Database.ExecuteSqlCommand("use Gambling_App; TRUNCATE Table tbl_confirmLeague;");
            _context.Database.ExecuteSqlCommand("use Gambling_App; TRUNCATE Table tbl_preUpcomingEvent;");

            foreach (var item in confrimLeague.LeagueList)
            {
                
                TblConfirmLeague confirmLeague = new TblConfirmLeague()
                {
                    LeagueId = item.LeagueId,
                    RapidLeagueId = item.RapidLeagueId,
                    Active = true
                };
                _context.TblConfirmLeague.Add(confirmLeague);
                _context.SaveChanges();

                var result = _context.TblUpcomingEvent.Find(item.LeagueId);
                TblPreUpcomingEvent preUpcomingEvent = new TblPreUpcomingEvent
                {
                    RapidEventId = result.RapidEventId,
                    LeagueId = result.LeagueId,
                    HomeTeamId = result.HomeTeamId,
                    AwayTeamId = result.AwayTeamId,
                    EventDate = result.EventDate,
                    EventTime = result.EventTime,
                    Active = result.Active
                };
                _context.TblPreUpcomingEvent.Add(preUpcomingEvent);
                _context.SaveChanges();
            }         
            return Ok(new { message = "Successfully Inserted" });
        }

        //Check Id exists in league table
        private bool TblLeagueExists(decimal id)
        {
            return _context.TblLeague.Any(e => e.LeagueId == id);
        }

        //Check Id exists in Confirm league table
        private bool TblConfirmLeagueExists(decimal id)
        {
            return _context.TblConfirmLeague.Any(e => e.LeagueId== id);
        }
    }
}
