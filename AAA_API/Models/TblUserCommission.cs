using System;
using System.Collections.Generic;

namespace AAA_API.Models
{
    public partial class TblUserCommission
    {
        public decimal UserCommissionId { get; set; }
        public int? UserCommissionTypeId { get; set; }
        public decimal? UserId { get; set; }
        public decimal? UserCommission { get; set; }
        public decimal? SubUserId { get; set; }
        public decimal? SubUserCommission { get; set; }
    }
}
