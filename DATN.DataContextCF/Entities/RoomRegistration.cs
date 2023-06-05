using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextCF.Entities
{
    public class RoomRegistration
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime DateRegistration { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Status { get; set; }

        //Khóa tham chiếu đến bảng khác
        public Room Room { get; set; }
        public Users User { get; set; }
    }
}
