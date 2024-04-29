using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SurtiPlus.Models
{
    public partial class Userv
    {
        [Key]
        public int UserId { get; set; }
        public string? AdminNameId { get; set; }
        public string? UserPassword { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}