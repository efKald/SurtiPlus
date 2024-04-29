using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SurtiPlus.Models
{
    public partial class Adminv
    {
        [Key]
        public string? AdminNameId { get; set; }
        public string? AdminPassword { get; set; }
    }
}