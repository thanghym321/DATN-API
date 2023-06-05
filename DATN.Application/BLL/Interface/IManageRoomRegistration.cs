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
        Task<PageResult<RoomRegistrationViewModel>> GetAllPaging(int pageindex, int pagesize, int Status);
        Task<RoomRegistrationViewModel> GetById(int Id);
        Task<List<RoomRegistrationViewModel>> GetByIdUser(int Id);
        Task<int> Create(RoomRegistrationModel request);
        Task<int> Confirm(int Id);
        //public void Start();
        //Task<int> Delete(int Id);
    }
}
