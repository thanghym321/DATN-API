using DATN.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL.Interface
{
    public interface IManageRoomRegistration
    {
        Task<List<RoomRegistrationViewModel>> Get();
        Task<RoomRegistrationViewModel> GetById(int Id);
        Task<PageResult<RoomRegistrationViewModel>> GetAllPaging(RoomRegistrationModel request);
        Task<int> Create(RoomRegistrationModel request);
        Task<int> Confirm(int Id);
        Task<int> SendMailLeave(string Email, int Id);
        Task<int> VerifyLeave(string Code, int Id);
    }
}
