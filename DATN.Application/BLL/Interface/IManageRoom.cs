using DATN.Application.Common;
using DATN.DataContextCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL.Interface
{
    public interface IManageRoom
    {
        Task<List<RoomViewModel>> Get(string Name);
        Task<RoomByIdViewModel> GetRoomByUserId(int Id);
        Task<PageResult<Users>> GetAllPagingUserInRoom(int pageindex, int pagesize, int Id);
        Task<PageResult<RoomViewModel>> GetAllPaging(RoomModel request);
        Task<RoomViewModel> GetById(int Id);
        Task<int> Create(Room request);
        Task<int> Update(Room request);
        Task<int> Delete(int Id);
    }
}
