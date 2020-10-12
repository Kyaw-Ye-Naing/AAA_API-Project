using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AAA_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AAA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        private readonly Gambling_AppContext _context;
        public CalculationController(Gambling_AppContext context)
        {
            _context = context;
        }

        // CREATING USER'S GAMBLING INFO FROM MOBILE : api/Calculation/betting
        [HttpPost]
        [Route("betting")]
        [Authorize(Policy = "EndUser")]
        public IActionResult BettingPost(UserBetting userBetting)
        {
            var user_id = User.FindFirst("userId")?.Value;
            TblGambling tblGambling = new TblGambling
            {
                PostingNo = userBetting.PostingNo,
                GamblingTypeId = userBetting.GamblingTypeId,
                EventId = userBetting.EventId,
                RapidEventId = userBetting.RapidEventId,
                TeamCount = userBetting.TeamCount,
                Amount = userBetting.Amount,
                UserId = userBetting.UserId,
                Active = true
            };
            _context.TblGambling.Add(tblGambling);
            _context.SaveChanges();
            var gamblingId = tblGambling.GamblingId;

            foreach (var item in userBetting.Details)
            {
                TblGamblingDetails gamblingDetails = new TblGamblingDetails
                {
                    GamblingId = gamblingId,
                    LeagueId = item.LeagueId,
                    FootballTeamId = item.FootballTeamId,
                    Under = item.Under,
                    Overs = item.Overs,
                    BodyOdd = item.BodyOdd,
                    GoalOdd = item.GoalOdd
                };
                _context.TblGamblingDetails.Add(gamblingDetails);
                _context.SaveChanges();
            }

            TblUserPosting userPosting = new TblUserPosting() {
                PostingNo = userBetting.PostingNo,
                UserId = userBetting.UserId,
                Inward = 0,
                Outward= userBetting.Amount,
                Active=true,
                CreatedBy =Decimal.Parse(user_id),
                CreatedDate =DateTime.Now
};
            _context.TblUserPosting.Add(userPosting);
            _context.SaveChanges();

            var parentId = _context.TblUser.Where(a => a.UserId == userBetting.UserId).First().CreatedBy;
            TblUserPosting parent_userPosting = new TblUserPosting()
            {
                PostingNo = userBetting.PostingNo,
                UserId = parentId,
                Inward = userBetting.Amount,
                Outward = 0,
                Active = true,
                CreatedBy = Decimal.Parse(user_id),
                CreatedDate = DateTime.Now
            };
            _context.TblUserPosting.Add(parent_userPosting);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Betting Successfully"
            }); 
        }
    }
}
