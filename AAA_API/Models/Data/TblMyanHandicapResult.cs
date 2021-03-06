﻿using System;
using System.Collections.Generic;

namespace AAA_API.Models.Data
{
    public partial class TblMyanHandicapResult
    {
        public decimal MyanHandicapResultId { get; set; }
        public string Body { get; set; }
        public string Goal { get; set; }
        public decimal? OverTeamId { get; set; }
        public decimal? UnderTeamId { get; set; }
        public decimal? HomeTeamId { get; set; }
        public decimal? AwayTeamId { get; set; }
        public decimal? LeagueId { get; set; }
        public decimal? RapidEventId { get; set; }
    }
}
