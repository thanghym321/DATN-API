using DATN.Application.Common;
using DATN.DataContextCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL.Interface
{
    public interface IManageCampus
    {
        Task<List<Campus>> Get();
        Task<PageResult<Campus>> GetAllPaging(int pageindex, int pagesize, string Name);
        Task<Campus> GetById(int Id);
        Task<int> Create(Campus request);
        Task<int> Update(Campus request);
        Task<int> Delete(int Id);
    }
}
