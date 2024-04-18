using System;
using System.Collections.Generic;

namespace MVCCRUD.Models
{
    public partial class Productv
    {
        public int ProductId { get; set; }
        public string? AdminName { get; set; }
        public string? ProductType { get; set; }
        public int? Amount { get; set; }
    }
}