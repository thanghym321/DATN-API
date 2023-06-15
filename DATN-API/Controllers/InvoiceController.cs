using DATN.Application.BLL.Interface;
using DATN.Application.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DATN_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IManageInvoice _manageInvoice;
        public InvoiceController(IManageInvoice manageInvoice)
        {
            _manageInvoice = manageInvoice;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> getbyid(int Id)
        {
            var result = await _manageInvoice.GetById(Id);
            if (result == null)
            {
                return BadRequest("Cannot find");
            }
            return Ok(result);

        }


        [HttpGet]
        public async Task<IActionResult> getallpaging([FromQuery] RoomInvoiceModel request)
        {
            var result = await _manageInvoice.GetAllPaging(request);
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }     
    }
}
