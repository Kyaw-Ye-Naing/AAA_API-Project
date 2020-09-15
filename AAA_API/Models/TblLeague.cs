using System;
using System.Collections.Generic;

namespace AAA_API.Models
{
    public partial class TblLeague
    {
        public decimal LeagueId { get; set; }
        public string LeagueName { get; set; }
        public decimal? RapidLeagueId { get; set; }
        public bool? Active { get; set; }
    }
}
