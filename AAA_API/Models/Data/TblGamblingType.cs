using System;
using System.Collections.Generic;

namespace AAA_API.Models.Data
{
    public partial class TblGamblingType
    {
        public int GamblingTypeId { get; set; }
        public string GamblingType { get; set; }
        public int? MinBetAmount { get; set; }
        public int? MaxBetAmount { get; set; }
    }
}
