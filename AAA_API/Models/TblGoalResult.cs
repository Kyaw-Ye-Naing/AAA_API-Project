using System;
using System.Collections.Generic;

namespace AAA_API.Models
{
    public partial class TblGoalResult
    {
        public decimal GoalResultId { get; set; }
        public string RapidEventId { get; set; }
        public int? HomeResult { get; set; }
        public int? AwayResult { get; set; }
    }
}
