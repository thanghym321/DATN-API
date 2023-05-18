using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class Report
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Content { get; set; }
        public DateTime? DateCreated { get; set; }

        public virtual Users User { get; set; }
    }
}
