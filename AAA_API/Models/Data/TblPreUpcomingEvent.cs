﻿using System;
using System.Collections.Generic;

namespace AAA_API.Models.Data
{
    public partial class TblPreUpcomingEvent
    {
        public decimal PreUpcommingEventId { get; set; }
        public decimal? RapidEventId { get; set; }
        public decimal? LeagueId { get; set; }
        public decimal? HomeTeamId { get; set; }
        public decimal? AwayTeamId { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime? EventTime { get; set; }
        public bool? Active { get; set; }
    }
}
