using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SurtiPlus.Models
{
    public partial class Productv
    {
        [Key]
        public int ProductId { get; set; }
        public string? AdminNameId { get; set; }
        public string? ProductType { get; set; }
        public int? Amount { get; set; }
    }
}