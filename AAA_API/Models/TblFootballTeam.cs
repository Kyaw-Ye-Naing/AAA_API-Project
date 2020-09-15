﻿using System;
using System.Collections.Generic;

namespace AAA_API.Models
{
    public partial class TblFootballTeam
    {
        public decimal FootballTeamId { get; set; }
        public decimal? RapidTeamId { get; set; }
        public string FootballTeam { get; set; }
        public string FootballTeamMyan { get; set; }
        public decimal? LeagueId { get; set; }
        public bool? Active { get; set; }
        public decimal? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
