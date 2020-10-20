using System;
using System.Collections.Generic;

namespace AAA_API.Models.Data
{
    public partial class TblUserBalance
    {
        public decimal UserBalanceId { get; set; }
        public int? TransactionTypeId { get; set; }
        public string PostingNo { get; set; }
        public decimal? UserId { get; set; }
        public int? Inward { get; set; }
        public int? Outward { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
