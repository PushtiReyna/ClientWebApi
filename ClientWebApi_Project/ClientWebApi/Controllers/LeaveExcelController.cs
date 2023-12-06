using ClientWebApi.ViewModel.ImportExcelData;
using ClientWebApi.ViewModel.Salary;
using DTO.ImportExcelData;
using DTO.Salary;
using Helper.CommonModel;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;

namespace ClientWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveExcelController : ControllerBase
    {
        public readonly IExcel _excel;

        public LeaveExcelController(IExcel excel)
        {
            _excel = excel;
        }

        [HttpPost]
        [Route("ImportExcelData")]
        public CommonResponse ImportExcelData([FromForm] GetDataReqViewModel getDataReqViewModel)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _excel.ImportExcelData(getDataReqViewModel.Adapt<GetDataReqDTO>());
                GetDataResDTO getDataResDTO = response.Data;
                response.Data = getDataResDTO.Adapt<GetDataResViewModel>();
            }
            catch { throw; }
            return response;
        }

        [HttpPut]
        [Route("EarnedMonthlyLeave")]
        public CommonResponse EarnedMonthlyLeave()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _excel.EarnedMonthlyLeave();
            }
            catch { throw; }
            return response;
        }

    }
}
