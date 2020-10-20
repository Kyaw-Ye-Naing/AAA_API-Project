using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AAA_API.Models.SP.LogInSP
{
    public class UserCommissionDetails
    {
        public int CommissionTypeId { get; set; }
        public int GamblingTypeId { get; set; }
        public string GamblingType { get; set; }
        public int MinBetAmount { get; set; }
        public int MaxBetAmount { get; set; }
        public int? BetTeamCount { get; set; }
        public string CommissionType { get; set; }
        public decimal? UserId { get; set; }
        public decimal? UserCommission { get; set; }
        public decimal? SubUserId { get; set; }
        public decimal? SubUserCommission { get; set; }
    }
}
