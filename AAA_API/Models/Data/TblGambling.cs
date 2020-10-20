using System;
using System.Collections.Generic;

namespace AAA_API.Models.Data
{
    public partial class TblGambling
    {
        public decimal GamblingId { get; set; }
        public int? TransactionTypeId { get; set; }
        public string PostingNo { get; set; }
        public int? GamblingTypeId { get; set; }
        public decimal? EventId { get; set; }
        public decimal? RapidEventId { get; set; }
        public int? TeamCount { get; set; }
        public int? Amount { get; set; }
        public bool? Active { get; set; }
        public decimal? UserId { get; set; }
    }
}
