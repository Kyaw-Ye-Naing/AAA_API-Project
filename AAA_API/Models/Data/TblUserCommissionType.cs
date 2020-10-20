using System;
using System.Collections.Generic;

namespace AAA_API.Models.Data
{
    public partial class TblUserCommissionType
    {
        public int CommissionTypeId { get; set; }
        public int? GamblingTypeId { get; set; }
        public int? BetTeamCount { get; set; }
        public string CommissionType { get; set; }
    }
}
