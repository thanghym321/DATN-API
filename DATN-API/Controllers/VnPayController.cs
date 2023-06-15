using DATN.Application.BLL.Interface;
using DATN.Application.VnPay;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace DATN_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VnPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public VnPayController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }
        [HttpPost]
        public IActionResult CreatePaymentUrl(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
            if (url == null)
            {
                return BadRequest("Get Failed");
            }
            return Ok(url);
        }
        [HttpGet]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null)
            {
                return BadRequest("Get Failed");
            }
            return Ok(response);
        }
    }
}
