using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextCF.Entities
{
    public class Room
    {

        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Bed { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }

        //Khóa tham chiếu đến bảng khác
        public Building Building { get; set; }

        //Danh sách chưa các tập con
        public List<RoomRegistration> RoomRegistrations { get; set; }
        public List<MeterReading> MeterReadings { get; set; }
        public List<Invoice> Invoices { get; set;}

    }
}
