using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AAA_API.Models
{
    public class UserBetting
    {
        public string PostingNo { get; set; }
        public int? GamblingTypeId { get; set; }
        public decimal? EventId { get; set; }
        public decimal? RapidEventId { get; set; }
        public int? TeamCount { get; set; }
        public int? Amount { get; set; }
        public bool? Active { get; set; }
        public decimal? UserId { get; set; }
        public UserBettingDetails[] Details { get; set; }
    }
    public class UserBettingDetails
    {
        public decimal? LeagueId { get; set; }
        public decimal? FootballTeamId { get; set; }
        public bool? Under { get; set; }
        public bool? Overs { get; set; }
        public string BodyOdd { get; set; }
        public string GoalOdd { get; set; }
    }

}
