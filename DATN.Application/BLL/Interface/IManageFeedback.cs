using DATN.Application.Common;
using DATN.DataContextCF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL.Interface
{
    public interface IManageFeedback
    {
        Task<List<Feedback>> Get();
        Task<PageResult<Feedback>> GetAllPaging(int pageindex, int pagesize);
        Task<Feedback> GetById(int Id);
        Task<int> Create(Feedback request);
        Task<int> Update(Feedback request);
        Task<int> Delete(int Id);
    }
}
