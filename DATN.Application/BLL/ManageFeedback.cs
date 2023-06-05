using DATN.Application.BLL.Interface;
using DATN.Application.Common;
using DATN.Application.User;
using DATN.DataContextCF.EF;
using DATN.DataContextCF.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DATN.Application.BLL
{
    public class ManageFeedback : IManageFeedback
    {
        private readonly DATN_CFContext _context;
        public ManageFeedback(DATN_CFContext context)
        {
            _context = context;
        }

        public async Task<List<Feedback>> Get()
        {
            var query = from a in _context.Feedbacks
                        join b in _context.Reports on a.ReportId equals b.Id
                        select new { a, b };
            return await query.Select(x => new Feedback()
            {
                Id = x.a.Id,
                UserId = x.a.UserId,
                ReportId = x.a.ReportId,
                Title = x.a.Title,
                Content = x.a.Content,
                DateCreated = x.a.DateCreated
            }
            ).ToListAsync();
        }
        public async Task<PageResult<Feedback>> GetAllPaging(int pageindex, int pagesize)
        {
            var query = from a in _context.Feedbacks
                        join b in _context.Users on a.UserId equals b.Id
                        select new { a, b };

            int totalRow = await query.CountAsync();
            var data = await query.Skip((pageindex - 1) * pagesize).Take(pagesize)
            .Select(x => new Feedback()
            {
                Id = x.a.Id,
                UserId = x.a.UserId,
                ReportId = x.a.ReportId,
                Title = x.a.Title,
                Content = x.a.Content,
                DateCreated = x.a.DateCreated

            }).ToListAsync();

            var pageResult = new PageResult<Feedback>()
            {
                TotalItem = totalRow,
                Items = data,
            };

            return pageResult;

        }
        public async Task<Feedback> GetById(int Id)
        {
            var query = from a in _context.Feedbacks
                        join b in _context.Users on a.UserId equals b.Id
                        select new Feedback
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            ReportId = a.ReportId,
                            Title = a.Title,
                            Content = a.Content,
                            DateCreated = a.DateCreated
                        };

            var obj = await query.SingleOrDefaultAsync(x => x.UserId == Id);

            return obj;
        }
        public async Task<int> Create(Feedback request)
        {
            var obj = new Feedback()
            {
                UserId = request.UserId,
                ReportId= request.ReportId,
                Title = request.Title,
                Content = request.Content,
            };

            _context.Feedbacks.Add(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
        public async Task<int> Update(Feedback request)
        {
            var obj = await _context.Feedbacks.FindAsync(request.Id);

            obj.UserId = request.UserId;
            obj.ReportId = request.ReportId;
            obj.Title = request.Title;
            obj.Content = request.Content;

            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> Delete(int Id)
        {
            var obj = await _context.Feedbacks.FindAsync(Id);

            _context.Feedbacks.Remove(obj);
            await _context.SaveChangesAsync();

            return 1;
        }
    }
}
