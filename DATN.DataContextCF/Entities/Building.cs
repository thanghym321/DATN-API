using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextCF.Entities
{
    public class Building
    {
        public int Id { get; set; }
        public int CampusId { get; set; }
        public string Name { get; set; }
        public int Floor { get; set; }
        public int Room { get; set; }

        //Khóa tham chiếu đến bảng khác
        public Campus Campus { get; set; }

        //Danh sách chưa các tập con
        public List<Room> Rooms { get; set; }
    }
}
