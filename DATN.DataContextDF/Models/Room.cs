using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class Room
    {
        public Room()
        {
            MeterReadings = new HashSet<MeterReading>();
            RoomRegistrations = new HashSet<RoomRegistration>();
        }

        public int Id { get; set; }
        public int? BuildingId { get; set; }
        public string Type { get; set; }
        public int? Bed { get; set; }
        public decimal? SemesterPrice { get; set; }
        public decimal? SummerPrice { get; set; }
        public string Status { get; set; }

        public virtual Building Building { get; set; }
        public virtual ICollection<MeterReading> MeterReadings { get; set; }
        public virtual ICollection<RoomRegistration> RoomRegistrations { get; set; }
    }
}
