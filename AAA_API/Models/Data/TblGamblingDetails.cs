﻿using System;
using System.Collections.Generic;

namespace AAA_API.Models.Data
{
    public partial class TblGamblingDetails
    {
        public decimal GamblingDetailsId { get; set; }
        public decimal? GamblingId { get; set; }
        public decimal? LeagueId { get; set; }
        public decimal? FootballTeamId { get; set; }
        public bool? Under { get; set; }
        public bool? Overs { get; set; }
        public string BodyOdd { get; set; }
        public string GoalOdd { get; set; }
    }
}
