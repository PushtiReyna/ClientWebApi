using ClientWebApi.ViewModel.Attendance;
using ClientWebApi.ViewModel.ImportExcelData;
using DTO.Attendance;
using DTO.ImportExcelData;
using Helper.CommonModel;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;

namespace ClientWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        public readonly IAttendance _attendance;

        public AttendanceController(IAttendance attendance)
        {
            _attendance = attendance;
        }

        [HttpPost]
        [Route("Attendance")]
        public CommonResponse Attendance(AttendanceReqViewModel attendanceReqViewModel)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _attendance.Attendance(attendanceReqViewModel.Adapt<AttendanceReqDTO>());
                AttendanceResDTO attendanceResDTO = response.Data;
                response.Data = attendanceResDTO.Adapt<AttendanceResViewModel>();
            }
            catch { throw; }
            return response;
        }
    }
}
