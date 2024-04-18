using System;
using System.Collections.Generic;

namespace MVCCRUD.Models
{
    public partial class Userv
    {
        public int UserId { get; set; }
        public string? AdminName { get; set; }
        public string? UserPassword { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}