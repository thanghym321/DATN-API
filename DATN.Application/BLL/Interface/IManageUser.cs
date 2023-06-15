using DATN.Application.Common;
using DATN.Application.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL.Interface
{
    public interface IManageUser
    {
        Task<int> SendMailResetPassword(string Email);
        Task<int> SendMailChangePassword(string Email, int Id);
        Task<int> VerifyChangePassWord(string Code, string NewPassword, int Id);
        Task<int> Register(UserModel request);
        Task<int> VerifyRegister(string Code, int Id);
        UserViewModel Authenticate(string username, string password);
        Task<List<UserViewModel>> Get();
        Task<PageResult<UserViewModel>> GetAllPaging(int pageindex, int pagesize, string UserName, string Name, int Role);
        Task<UserViewModel> GetById(int Id);
        Task<int> Create(UserModel request);
        Task<int> Update(UserModel request);
        Task<int> Delete(int Id);
    }
}
