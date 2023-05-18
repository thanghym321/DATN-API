using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class MeterReading
    {
        public int Id { get; set; }
        public int? RoomId { get; set; }
        public int? ElectricityIndex { get; set; }
        public int? WaterIndex { get; set; }
        public DateTime? Date { get; set; }

        public virtual Room Room { get; set; }
    }
}
