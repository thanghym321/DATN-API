using DATN.Application.Common;
using DATN.DataContextCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL.Interface
{
    public interface IManageElectricityWaterRate
    {
        Task<List<ElectricityWaterRate>> Get();
        Task<PageResult<ElectricityWaterRate>> GetAllPaging(int pageindex, int pagesize);
        Task<ElectricityWaterRate> GetById(int Id);
        Task<int> Create(ElectricityWaterRate request);
        Task<int> Update(ElectricityWaterRate request);
        Task<int> Delete(int Id);
    }
}
