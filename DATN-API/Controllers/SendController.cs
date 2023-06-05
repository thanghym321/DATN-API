using DATN.Application.BLL.Interface;
using DATN.DataContextCF.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendController : ControllerBase
    {
        private readonly ISendMailService _manageBuilding;
        public SendController(ISendMailService manageBuilding)
        {
            _manageBuilding = manageBuilding;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(MailContent mailContent)
        {
            MailContent content = new MailContent
            {
                To = mailContent.To,
                Subject = mailContent.Subject,
                Body = mailContent.Body
            };

            await _manageBuilding.SendMail(content);
            return Ok("Thành công");
        }
    }
}
