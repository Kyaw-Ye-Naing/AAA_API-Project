using System;
using System.Collections.Generic;

namespace AAA_API.Models
{
    public partial class TblUserBalance
    {
        public decimal UserBalanceId { get; set; }
        public string PostingNo { get; set; }
        public decimal? UserId { get; set; }
        public int? Inward { get; set; }
        public int? Outward { get; set; }
    }
}
