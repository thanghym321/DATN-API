using DATN.Application.BLL;
using DATN.Application.BLL.Interface;
using DATN.Application.User;
using DATN.DataContextCF.EF;
using DATN.DataContextCF.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //mail
            services.AddOptions();                                         // Kích hoạt Options
            var mailsettings = Configuration.GetSection("MailSettings");  // đọc config
            services.Configure<MailSettings>(mailsettings);                // đăng ký để Inject




            //db
            services.AddDbContext<DATN_CFContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DATN_CF")));

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //services
            services.AddTransient<IManageUser, ManageUser>();
            services.AddTransient<IManageCampus, ManageCampus>();
            services.AddTransient<IManageBuilding, ManageBuilding>();
            services.AddTransient<IManageRoom, ManageRoom>();
            services.AddTransient<IManageElectricityWaterRate, ManageElectricityWaterRate>();
            services.AddTransient<IManageFeedback, ManageFeedback>();
            services.AddTransient<IManageMeterReading, ManageMeterReading>();
            services.AddTransient<IManageReport, ManageReport>();
            services.AddTransient<IManageService, ManageService>();
            services.AddTransient<ISendMailService, SendMailService>();
            services.AddTransient<IManageRoomRegistration, ManageRoomRegistration>();
            services.AddTransient<IManageInvoice, ManageInvoice>();




            // Lấy tham chiếu đến IServiceProvider để có thể giải quyết các phụ thuộc
            var serviceProvider = services.BuildServiceProvider();

            // Gọi phương thức Start() của ManageRoomRegistration
            var invoice = serviceProvider.GetService<IManageInvoice>();
            invoice.Start();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DATN_API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            ////mail 
            //app.UseEndpoints(endpoints => {
            //    endpoints.MapGet("/", async context => {
            //        await context.Response.WriteAsync("Hello World!");
            //    });

            //    endpoints.MapGet("/testmail", async context => {

            //        // Lấy dịch vụ sendmailservice
            //        var sendmailservice = context.RequestServices.GetService<ISendMailService>();

            //        MailContent content = new MailContent
            //        {
            //            To = "xuanthulab.net@gmail.com",
            //            Subject = "Kiểm tra thử",
            //            Body = "<p><strong>Xin chào xuanthulab.net</strong></p>"
            //        };

            //        await sendmailservice.SendMail(content);
            //        await context.Response.WriteAsync("Send mail");
            //    });

            //});

            //cors them dau tien
            app.UseCors(builder => builder
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DATN_API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
