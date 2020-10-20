using System;
using System.Collections.Generic;
using System.Linq;
using AAA_API.Models;
using AAA_API.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "EndUser")]
    public class UserBettingController : ControllerBase
    {
        private readonly Gambling_AppContext _context;
        public UserBettingController(Gambling_AppContext context)
        {
            _context = context;
        }

        // CREATING USER'S GAMBLING INFO FROM MOBILE : api/UserBetting
        [HttpPost]
        public IActionResult BettingPost(UserBetting userBetting)
        {
            var user_id = User.FindFirst("userId")?.Value;
            TblGambling tblGambling = new TblGambling
            {
                PostingNo = userBetting.PostingNo,
                TransactionTypeId=9,
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
                TransactionTypeId=9,
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
                TransactionTypeId = 10,
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

        //SHOWING MYANMAR BODY HANDICAP AND GOAL HANDICAP TO USERS : api/UserBetting
        [HttpGet]
        public IActionResult ShowHandicap()
        {
            TblMyanHandicapResult myanHandicapResult = new TblMyanHandicapResult();

            var result = (from m in _context.TblMyanHandicapResult 
                          join l in _context.TblLeague 
                          on m.LeagueId equals l.LeagueId 
                          select new
                          {
                              BodyHandicap =m.Body,
                              GoalHandicap=m.Goal,
                              OverTeam=_context.TblFootballTeam.Where(a=>a.FootballTeamId==m.OverTeamId).First().FootballTeam,
                              UnderTeam = _context.TblFootballTeam.Where(a => a.FootballTeamId == m.UnderTeamId).First().FootballTeam,
                              HomeTeam = _context.TblFootballTeam.Where(a => a.FootballTeamId == m.HomeTeamId).First().FootballTeam,
                              AwayTeam = _context.TblFootballTeam.Where(a => a.FootballTeamId == m.AwayTeamId).First().FootballTeam,
                              League = l.LeagueName,
                              RapidTeamId = m.RapidEventId
                              // other assignments
                          }).ToList();
            return Ok(result);
        }
    }
}
