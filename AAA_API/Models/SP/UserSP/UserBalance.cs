﻿using System;
using System.Collections.Generic;

namespace AAA_API.Models
{
    public partial class UserBalance
    {
        public decimal? UserId { get; set; }

        public string UserName { get; set; }
        public decimal? Inward { get; set; }
        public decimal? Outward { get; set; }
        public decimal? Balance { get; set; }
    }
}
