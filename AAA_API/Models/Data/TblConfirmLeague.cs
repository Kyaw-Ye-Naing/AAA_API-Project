using System;
using System.Collections.Generic;

namespace AAA_API.Models.Data
{
    public partial class TblConfirmLeague
    {
        public decimal ConfirmLeagueId { get; set; }
        public decimal? LeagueId { get; set; }
        public decimal? RapidLeagueId { get; set; }
        public bool? Active { get; set; }
    }
}
