using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AAA_API.Models
{
    public class MinMax
    {
        public decimal Id{ get; set; }
        public int GamblingTypeId { get; set; }
        public int? MinBetAmount { get; set; }
        public int? MaxBetAmount { get; set; }
    }
}
