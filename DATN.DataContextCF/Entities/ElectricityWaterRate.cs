using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextCF.Entities
{
    public partial class ElectricityWaterRate
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Tier { get; set; }
        public int StartAmount { get; set; }
        public int EndAmount { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
    }
}
