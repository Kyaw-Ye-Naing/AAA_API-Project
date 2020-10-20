using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AAA_API.Models.SP.LogInSP
{
    public class UserLogin
    {
        public decimal UserId { get; set; }
        public string Username { get; set; }
        public int MemberCount { get; set; }
        public Boolean? Lock { get; set; }
        public int? RoleId { get; set; }
        public string Role { get; set; }
        public string Mobile { get; set; }
        public decimal? SharePercent { get; set; }
        public decimal? BetLimitForMix { get; set; }
        public decimal? BetLimitForSingle { get; set; }
        public decimal? Inward { get; set; }
        public decimal? Outward { get; set; }
        public decimal? Balance { get; set; }
       
    }
}
