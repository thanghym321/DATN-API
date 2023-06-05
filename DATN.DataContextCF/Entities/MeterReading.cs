using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextCF.Entities
{
    public class MeterReading
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int ElectricityIndex { get; set; }
        public int WaterIndex { get; set; }
        public DateTime Date { get; set; }

        //Khóa tham chiếu đến bảng khác
        public Room Room { get; set; }
    }
}
