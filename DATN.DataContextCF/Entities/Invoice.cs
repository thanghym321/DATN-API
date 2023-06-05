using System;
using System.Collections.Generic;
using System.Xml.Schema;

#nullable disable

namespace DATN.DataContextCF.Entities
{
    public partial class Invoice
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DatePayment { get; set; }
        public decimal Total { get; set; }
        public int Status { get; set; }

        //Khóa tham chiếu đến bảng khác
        public Users User { get; set; }
        public Room Room { get; set; }

        //Danh sách
        public List<UtilityBill> UtilityBills { get; set;}

    }
}
