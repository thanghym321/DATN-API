using DATN.Application.BLL;
using DATN.Application.BLL.Interface;
using DATN.DataContextCF.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DATN_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoomRegistrationController : ControllerBase
    {
        private readonly IManageRoomRegistration _manageRoomRegistration;
        public RoomRegistrationController(IManageRoomRegistration manageRoomRegistration)
        {
            _manageRoomRegistration = manageRoomRegistration;
        }
        [HttpGet]
        public async Task<IActionResult> getallpaging([FromQuery] RoomRegistrationModel request)
        {
            var result = await _manageRoomRegistration.GetAllPaging(request);
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] RoomRegistrationModel request)
        {
            var result = await _manageRoomRegistration.Create(request);
            if (result == 1)
            {
                return Ok(1);
            }
            if (result == 2) 
            {
                return Ok(2);
            }

            return BadRequest("Create Failed");

        }
        [HttpPut]
        public async Task<IActionResult> confirm([FromBody] int Id)
        {
            var result = await _manageRoomRegistration.Confirm(Id);
            if (result == 1)
            {
                return Ok(new { data = "OK" });
            }

            return BadRequest("Confirm Failed");

        }

        [HttpGet]
        public async Task<IActionResult> SendMailLeave(string Email, int Id)
        {
            var result = await _manageRoomRegistration.SendMailLeave(Email,Id);
            if (result == 1)
            {
                return Ok(1);

            }
            return BadRequest("Send Failed");

        }
        [HttpGet]
        public async Task<IActionResult> VerifyLeave(string Code, int Id)
        {
            string htmlContent = "<html><body><h1>Tra phong thanh cong!</h1></body></html>";
            string htmlContent1 = "<html><body><h1>Ban van con hoa don chua thanh toan!</h1></body></html>";
            string htmlContent2 = "<html><body><h1>Duong dan khong con hieu luc!</h1></body></html>";


            var result = await _manageRoomRegistration.VerifyLeave(Code, Id);
            if (result == 1)
            {
                return Content(htmlContent, "text/html");

            }
            if (result == 2)
            {
                return Content(htmlContent1, "text/html");

            }
            return Content(htmlContent2, "text/html");
        }
    }
}
