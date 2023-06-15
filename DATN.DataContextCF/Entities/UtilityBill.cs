using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextCF.Entities
{
    public class UtilityBill
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string Type { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        //Khóa tham chiếu đến bảng khác
        public Invoice Invoice { get; set; }
    }
}
