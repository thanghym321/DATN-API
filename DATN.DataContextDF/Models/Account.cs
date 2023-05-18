using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextDF.Models
{
    public partial class Account
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }

        public virtual Users User { get; set; }
    }
}
