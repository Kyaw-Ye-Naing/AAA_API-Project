﻿using System;
using System.Collections.Generic;

namespace AAA_API.Models
{
    public partial class TblHandicap
    {
        public decimal HandicapId { get; set; }
        public decimal? RapidEventId { get; set; }
        public decimal? HomeOdd { get; set; }
        public string HomeHandicap { get; set; }
        public decimal? AwayOdd { get; set; }
        public string AwayHandicap { get; set; }
        public DateTime? EventDatetime { get; set; }
    }
}
