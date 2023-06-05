using DATN.Application.BLL.Interface;
using DATN.Application.Common;
using DATN.DataContextCF.EF;
using DATN.DataContextCF.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Application.BLL
{
    public class ManageReport : IManageReport
    {
        private readonly DATN_CFContext _context;
        public ManageReport(DATN_CFContext context)
        {
            _context = context;
        }

        public async Task<List<Report>> Get()
        {
            var query = from a in _context.Reports
                        join b in _context.Users on a.UserId equals b.Id
                        select new { a, b };
            return await query.Select(x => new Report()
            {
                Id = x.a.Id,
                UserId = x.a.UserId,
                Title = x.a.Title,
                Content = x.a.Content,
                DateCreated = x.a.DateCreated
            }
            ).ToListAsync();
        }
        public async Task<PageResult<Report>> GetAllPaging(int pageindex, int pagesize)
        {
            var query = from a in _context.Reports
                        join b in _context.Users on a.UserId equals b.Id
                        select new { a, b };

            int totalRow = await query.CountAsync();
            var data = await query.Skip((pageindex - 1) * pagesize).Take(pagesize)
            .Select(x => new Report()
            {
                Id = x.a.Id,
                UserId = x.a.UserId,
                Title = x.a.Title,
                Content = x.a.Content,
                DateCreated = x.a.DateCreated

            }).ToListAsync();

            var pageResult = new PageResult<Report>()
            {
                TotalItem = totalRow,
                Items = data,
            };

            return pageResult;

        }
        public async Task<Report> GetById(int Id)
        {
            var query = from a in _context.Reports
                        join b in _context.Users on a.UserId equals b.Id
                        select new Report
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            Title = a.Title,
                            Content = a.Content,
                            DateCreated = a.DateCreated
                        };

            var obj = await query.SingleOrDefaultAsync(x => x.UserId == Id);

            return obj;
        }
        public async Task<int> Create(Report request)
        {
            var obj = new Report()
            {
                UserId = request.UserId,
                Title = request.Title,
                Content = request.Content,
            };

            _context.Reports.Add(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
        public async Task<int> Update(Report request)
        {
            var obj = await _context.Reports.FindAsync(request.Id);

            obj.UserId = request.UserId;
            obj.Title = request.Title;
            obj.Content = request.Content;

            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> Delete(int Id)
        {
            var obj = await _context.Reports.FindAsync(Id);

            _context.Reports.Remove(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
    }
}
