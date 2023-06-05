using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.User
{
    public class UserViewModel
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int Status { get; set; }
        public int Role { get; set; }
        public int? BuildingId { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CitizenIdentityCard { get; set; }
        public string Token { get; set; }
    }
}
