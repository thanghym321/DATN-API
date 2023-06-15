using DATN.Application.BLL;
using DATN.Application.BLL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DATN_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatistic _Statistic;
        public StatisticController(IStatistic Statistic)
        {
            _Statistic = Statistic;
        }
        [HttpGet]
        public async Task<IActionResult> StatisticCampus()
        {
            var result = await _Statistic.StatisticCampus();
            if (result < 0)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> StatisticBuilding()
        {
            var result = await _Statistic.StatisticBuilding();
            if (result < 0)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> StatisticRoom()
        {
            var result = await _Statistic.StatisticRoom();
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> StatisticStudent()
        {
            var result = await _Statistic.StatisticStudent();
            if (result < 0)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> StatisticInvoice()
        {
            var result = await _Statistic.StatisticInvoice();
            if (result == null)
            {
                return BadRequest("Get Failed");
            }

            return Ok(result);
        }
    }
}
