using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextCF.Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReportId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }

        //Khóa tham chiếu đến bảng khác
        public Users User { get; set; }
        public Report Report { get; set; }
    }
}
