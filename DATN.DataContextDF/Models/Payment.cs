using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class Payment
    {
        public Payment()
        {
            DetailPayments = new HashSet<DetailPayment>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Type { get; set; }
        public DateTime? DatePayment { get; set; }
        public string Status { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<DetailPayment> DetailPayments { get; set; }
    }
}
