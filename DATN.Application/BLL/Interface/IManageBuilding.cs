using DATN.Application.Common;
using DATN.DataContextCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL.Interface
{
    public interface IManageBuilding
    {
        Task<List<BuildingViewModel>> Get();
        Task<PageResult<BuildingViewModel>> GetAllPaging(int pageindex, int pagesize, string Name);
        Task<BuildingViewModel> GetById(int Id);
        Task<int> Create(Building request);
        Task<int> Update(Building request);
        Task<int> Delete(int Id);
    }
}
