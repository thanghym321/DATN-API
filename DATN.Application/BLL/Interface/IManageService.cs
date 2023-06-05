using DATN.Application.Common;
using DATN.DataContextCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL.Interface
{
    public interface IManageService
    {
        Task<List<Service>> Get();
        Task<PageResult<Service>> GetAllPaging(int pageindex, int pagesize);
        Task<Service> GetById(int Id);
        Task<int> Create(Service request);
        Task<int> Update(Service request);
        Task<int> Delete(int Id);
    }
}
