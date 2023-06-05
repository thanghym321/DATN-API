using DATN.Application.Common;
using DATN.DataContextCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL.Interface
{
    public interface IManageMeterReading
    {
        Task<List<MeterReadingViewModel>> Get();
        Task<PageResult<MeterReadingViewModel>> GetAllPaging(int pageindex, int pagesize);
        Task<MeterReadingViewModel> GetById(int Id);
        Task<int> Create(MeterReading request);
        Task<int> Update(MeterReading request);
        Task<int> Delete(int Id);
    }
}
