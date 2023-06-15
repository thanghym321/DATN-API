using DATN.Application.BLL.Interface;
using DATN.Application.Common;
using DATN.Application.User;
using DATN.DataContextCF.EF;
using DATN.DataContextCF.Entities;
using DATN.DataContextCF.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace DATN.Application.BLL
{
    public class ManageUser : IManageUser
    {
        private readonly DATN_CFContext _context;
        private readonly AppSettings _appSettings;
        private readonly ISendMailService _sendMailService;
        private readonly IMemoryCache _cache;

        public ManageUser(IOptions<AppSettings> appSettings, DATN_CFContext context, ISendMailService sendMailService, IMemoryCache cache)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _sendMailService = sendMailService;
            _cache = cache;
        }
        public async Task<int> SendMailResetPassword(string Email)
        {
            var query = from a in _context.Accounts
                         join b in _context.Users on a.UserId equals b.Id
                         select new { a , b };

            string confirmationCode = Guid.NewGuid().ToString("N").Substring(0, 5);
            var user = await query.FirstOrDefaultAsync(x => x.b.Email == Email);
            if (user == null) 
            {
                return 2;
            }
            var account = await _context.Accounts.FindAsync(user.a.Id);
            account.PassWord= confirmationCode;
            await _context.SaveChangesAsync();

            var email = Email;
            var subject = "Xác nhận đổi mật khẩu";
            var htmlMessage = $"Mật khẩu của bạn là: {confirmationCode}";

            await _sendMailService.SendEmailAsync(email, subject, htmlMessage);
            return 1;
        }
        public async Task<int> SendMailChangePassword(string Email,int Id)
        {
            try
            {
                string confirmationCode = Guid.NewGuid().ToString("N").Substring(0, 5);

                var email = Email;
                var subject = "Xác nhận đổi mật khẩu";
                var htmlMessage = $"Mã xác nhận của bạn là: {confirmationCode}";

                await _sendMailService.SendEmailAsync(email, subject, htmlMessage);

                _cache.Set(Id, confirmationCode, TimeSpan.FromMinutes(5));

                return 1;
            }
            catch (Exception) { return 2; }
        }

        public async Task<int> VerifyChangePassWord(string Code, string NewPassword, int Id)
        {
            if (_cache.TryGetValue(Id, out string cachedVerificationCode))
            {
                // So sánh mã xác nhận nhập vào với mã xác nhận lưu trong cache
                if (Code == cachedVerificationCode)
                {
                    var user = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == Id);
                    user.PassWord = NewPassword;
                    await _context.SaveChangesAsync();

                    // Xóa mã xác nhận khỏi cache
                    _cache.Remove(Id);
                    return 1;
                }
            }
            return 2;
        }

        public UserViewModel Authenticate(string username, string password)
        {
            var result = from a in _context.Accounts
                         join b in _context.Users on a.UserId equals b.Id
                         select new UserViewModel
                         {
                             UserId = a.UserId,
                             UserName = a.UserName,
                             PassWord = a.PassWord,
                             Status = a.Status,
                             Role = a.Role,
                             BuildingId = b.BuildingId,
                             Name = b.Name,
                             DateOfBirth = b.DateOfBirth,
                             Gender = b.Gender,
                             Avatar = b.Avatar,
                             Address = b.Address,
                             Email = b.Email,
                             Phone = b.Phone,
                             CitizenIdentityCard = b.CitizenIdentityCard
                         };

            var user = result.SingleOrDefault(x => x.UserName == username && x.PassWord == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Name.ToString()),
                    new Claim(ClaimTypes.MobilePhone, user.Phone.ToString()),
                    new Claim(ClaimTypes.Email, user.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        public async Task<List<UserViewModel>> Get()
        {
            var query = from a in _context.Accounts
                        join b in _context.Users on a.UserId equals b.Id
                        select new
                        { a, b };
            return await query.Select(x => new UserViewModel()
            {
                UserId = x.a.UserId,
                UserName = x.a.UserName,
                PassWord = x.a.PassWord,
                Status = x.a.Status,
                Role = x.a.Role,
                BuildingId = x.b.BuildingId,
                Name = x.b.Name,
                DateOfBirth = x.b.DateOfBirth,
                Gender = x.b.Gender,
                Avatar = x.b.Avatar,
                Address = x.b.Address,
                Email = x.b.Email,
                Phone = x.b.Phone,
                CitizenIdentityCard = x.b.CitizenIdentityCard

            }).ToListAsync();
        }


        public async Task<PageResult<UserViewModel>> GetAllPaging(int pageindex, int pagesize, string UserName, string Name, int Role)
        {
            var query = from a in _context.Accounts
                        join b in _context.Users on a.UserId equals b.Id
                        select new { a, b };

            if (!string.IsNullOrEmpty(UserName))
            {
                query = query.Where(x => x.a.UserName.ToLower().Contains(UserName.ToLower()));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(x => x.b.Name.ToLower().Contains(Name.ToLower()));
            }
            if (Role!=-1)
            {
                query = query.Where(x => x.a.Role==Role);
            }

            int totalRow = await query.CountAsync();
            var data = await query.OrderByDescending(x => x.a.Id).Skip((pageindex - 1) * pagesize).Take(pagesize)
            .Select(x => new UserViewModel()
            {
                UserId = x.a.UserId,
                UserName = x.a.UserName,
                PassWord = x.a.PassWord,
                Status = x.a.Status,
                Role = x.a.Role,
                BuildingId = x.b.BuildingId,
                Name = x.b.Name,
                DateOfBirth = x.b.DateOfBirth,
                Gender = x.b.Gender,
                Avatar = x.b.Avatar,
                Address = x.b.Address,
                Email = x.b.Email,
                Phone = x.b.Phone,
                CitizenIdentityCard = x.b.CitizenIdentityCard

            }).ToListAsync();

            var pageResult = new PageResult<UserViewModel>()
            {
                TotalItem = totalRow,
                Items = data,
            };

            return pageResult;
        }

        public async Task<UserViewModel> GetById(int Id)
        {
            var query = from a in _context.Accounts
                        join b in _context.Users on a.UserId equals b.Id
                        select new UserViewModel
                        {
                            UserId = a.UserId,
                            UserName = a.UserName,
                            PassWord = a.PassWord,
                            Status = a.Status,
                            Role = a.Role,
                            BuildingId = b.BuildingId,
                            Name = b.Name,
                            DateOfBirth = b.DateOfBirth,
                            Gender = b.Gender,
                            Avatar = b.Avatar,
                            Address = b.Address,
                            Email = b.Email,
                            Phone = b.Phone,
                            CitizenIdentityCard = b.CitizenIdentityCard
                        };

            var user = await query.SingleOrDefaultAsync(x => x.UserId == Id);

            return user;
        }

        public async Task<int> Register(UserModel request)
        {
            var query = from a in _context.Accounts
                        join b in _context.Users on a.UserId equals b.Id
                        select new { a, b };

            var Active= await query.CountAsync(x => x.b.Email==request.user.Email && x.a.Status==(int)Status.Active);
            var InActive = await query.CountAsync(x => x.b.Email == request.user.Email && x.a.Status == (int)Status.INActive);


            if (Active > 0) { return 2; };
            if (InActive > 0) 
            {
                string confirmationCode1 = Guid.NewGuid().ToString("N").Substring(0, 5);

                var email1 = request.user.Email;
                var subject1 = "Xác nhận đăng ký";
                var htmlMessage1 = $"Nhấn vào đường link sau để xác nhận: http://localhost:5000/api/User/VerifyRegister?Id={request.user.Id}&Code={confirmationCode1}";

                await _sendMailService.SendEmailAsync(email1, subject1, htmlMessage1);

                _cache.Set(request.user.Id, confirmationCode1, TimeSpan.FromMinutes(30));
                return 3;
            }

            _context.Users.Add(request.user);
            await _context.SaveChangesAsync();

            int UserId = request.user.Id;
            request.account.UserId = UserId;
            _context.Accounts.Add(request.account);
            await _context.SaveChangesAsync();
            string confirmationCode = Guid.NewGuid().ToString("N").Substring(0, 5);

            var email = request.user.Email;
            var subject = "Xác nhận đăng ký";
            var htmlMessage = $"Nhấn vào đường link sau để xác nhận: http://localhost:5000/api/User/VerifyRegister?Id={request.user.Id}&Code={confirmationCode}";

            await _sendMailService.SendEmailAsync(email, subject, htmlMessage);

            _cache.Set(request.user.Id, confirmationCode, TimeSpan.FromMinutes(30));

            return 1;
        }

        public async Task<int> VerifyRegister(string Code, int Id)
        {
            if (_cache.TryGetValue(Id, out string cachedVerificationCode))
            {
                // So sánh mã xác nhận nhập vào với mã xác nhận lưu trong cache
                if (Code == cachedVerificationCode)
                {
                    var user = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == Id);
                    user.Status = (int)Status.Active;
                    await _context.SaveChangesAsync();

                    // Xóa mã xác nhận khỏi cache
                    _cache.Remove(Id);
                    return 1;
                }
            }
            return 2;
        }

        public async Task<int> Create(UserModel request)
        {
            _context.Users.Add(request.user);
            await _context.SaveChangesAsync();

            int UserId = request.user.Id;
            request.account.UserId = UserId;
            _context.Accounts.Add(request.account);
            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> Update(UserModel request)
        {
            var user = await _context.Users.FindAsync(request.user.Id);
        
            user.BuildingId = request.user.BuildingId;
            user.Name = request.user.Name;
            user.DateOfBirth = request.user.DateOfBirth;
            user.Gender = request.user.Gender;
            user.Avatar = request.user.Avatar;
            user.Address = request.user.Address;
            user.Email = request.user.Email;
            user.Phone = request.user.Phone;
            user.CitizenIdentityCard = request.user.CitizenIdentityCard;

            await _context.SaveChangesAsync();

            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == request.account.UserId);

            account.UserId = request.account.UserId;
            account.UserName = request.account.UserName;
            account.PassWord = request.account.PassWord;
            account.Status = request.account.Status;
            account.Role = request.account.Role;

            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> Delete(int Id)
        {
            var invoice = await _context.Invoices.CountAsync(x => x.UserId == Id && x.Status==(int)StatusInvoice.unpaid);
            if (invoice > 0)
            {
                return 2;
            }

            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.UserId == Id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == Id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return 1;
        }
    }
}
