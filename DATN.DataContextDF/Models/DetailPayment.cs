using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class DetailPayment
    {
        public int Id { get; set; }
        public int? PaymentId { get; set; }
        public int? RandomId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Total { get; set; }

        public virtual Payment Payment { get; set; }
    }
}
