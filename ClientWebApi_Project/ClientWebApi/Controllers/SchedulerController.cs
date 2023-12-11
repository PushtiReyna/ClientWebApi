using Helper.CommonModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;

namespace ClientWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {

        public readonly IScheduler _schedular;

        public SchedulerController(IScheduler schedular)
        {
            _schedular = schedular;
        }

        [HttpPost]
        [Route("EarnedMonthlyLeave")]
        public async Task<CommonResponse> EarnedMonthlyLeave()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _schedular.EarnedMonthlyLeave();
            }
            catch { throw; }
            return response;
        }
    }
}
