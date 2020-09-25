using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AAA_API.Models
{
    public class ChangePassword
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
