using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class ElectricityWaterRate
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? AmountUsed { get; set; }
        public decimal? Price { get; set; }
    }
}
