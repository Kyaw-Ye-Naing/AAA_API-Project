﻿using System;
using System.Collections.Generic;

namespace AAA_API.Models
{
    public partial class TblGamblingDetails
    {
        public decimal GamblingDetailsId { get; set; }
        public decimal? GamblingId { get; set; }
        public decimal? LeagueId { get; set; }
        public decimal? FootballTeamId { get; set; }
        public bool? Under { get; set; }
        public bool? Overs { get; set; }
        public decimal? Amount { get; set; }
    }
}
