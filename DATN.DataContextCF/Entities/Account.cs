using System;
using System.Collections.Generic;

namespace DATN.DataContextCF.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int Status { get; set; }
        public int Role { get; set; }

        //Khóa tham chiếu đến bảng khác
        public Users User { get; set; }
    }
}
