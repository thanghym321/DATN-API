using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextCF.Entities
{
    public class Users
    {
        public int Id { get; set; }
        public int? BuildingId { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CitizenIdentityCard { get; set; }

        //Khóa tham chiếu đến bảng khác
        public Account Account { get; set; }

        //Danh sách chưa các tập con
        public List<Feedback> Feedbacks { get; set; }
        public List<Invoice> Invoices { get; set; }
        public List<Report> Reports { get; set; }
        public List<RoomRegistration> RoomRegistrations { get; set; }
        public List<ServiceRegistration> ServiceRegistrations { get; set; }
    }
}
