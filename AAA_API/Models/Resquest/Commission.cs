using System;

namespace AAA_API.Models
{
    public class Commission
    {
        public decimal UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? Lock { get; set; }
        public int? RoleId { get; set; }
        public string Mobile { get; set; }
        public decimal? SharePercent { get; set; }
        public decimal? BetLimitForMix { get; set; }
        public decimal? BetLimitForSingle { get; set; }
        public decimal? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Commission1[] commission { get; set; }

        public class Commission1 {
          //  public  int CommissionId { get; set; }
            public int CommissionTypeId { get; set; }
            public decimal UserId { get; set; }
            public decimal SubUserCommission { get; set; }

        }

    }
}
