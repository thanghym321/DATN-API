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
        Task<PageResult<RoomViewModel>> GetAllPaging(int pageindex, int pagesize, int? Status);
        Task<RoomViewModel> GetById(int Id);
        Task<int> Create(Room request);
        Task<int> Update(Room request);
        Task<int> Delete(int Id);
    }
}
