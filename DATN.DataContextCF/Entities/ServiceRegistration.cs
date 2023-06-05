using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextCF.Entities
{
    public partial class ServiceRegistration
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int UserId { get; set; }
        public DateTime DateRegistration { get; set; }
        public int Status { get; set; }

        //Khóa tham chiếu đến bảng khác
        public Service Service { get; set; }
        public Users User { get; set; }
    }
}
