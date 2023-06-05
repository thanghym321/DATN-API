using DATN.Application.Common;
using DATN.DataContextCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL.Interface
{
    public interface IManageReport
    {
        Task<List<Report>> Get();
        Task<PageResult<Report>> GetAllPaging(int pageindex, int pagesize);
        Task<Report> GetById(int Id);
        Task<int> Create(Report request);
        Task<int> Update(Report request);
        Task<int> Delete(int Id);
    }
}
