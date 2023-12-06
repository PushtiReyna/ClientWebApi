using ClientWebApi.ViewModel.Salary;
using DTO.Salary;
using Helper.CommonModel;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;

namespace ClientWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        public readonly ISalary _salary;

        public SalaryController(ISalary salary)
        {
            _salary = salary;
        }

        [HttpPost]
        [Route("SalaryClient")]
        public CommonResponse SalaryClient(SalaryClientReqViewModel salaryClientReqViewModel)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _salary.SalaryClient(salaryClientReqViewModel.Adapt<SalaryClientReqDTO>());
                SalaryClientResDTO salaryClientReqDTO = response.Data;
                response.Data = salaryClientReqDTO.Adapt<SalaryClientResViewModel>();
            }
            catch { throw; }
            return response;
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("DownloadSalary")]
        public CommonResponse DownloadSalary(DownloadSalaryReqViewModel downloadSalaryReqViewModel)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _salary.DownloadSalary(downloadSalaryReqViewModel.Adapt<DownloadSalaryReqDTO>());
                DownloadSalaryResDTO downloadSalaryResDTO = response.Data;
                response.Data = downloadSalaryResDTO.Adapt<DownloadSalaryResViewModel>();
            }
            catch { throw; }
            return response;
        }

    }
}
