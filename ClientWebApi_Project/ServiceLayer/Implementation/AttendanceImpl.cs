using BusinessLayer;
using DTO.Attendance;
using Helper.CommonModel;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Implementation
{
    public class AttendanceImpl : IAttendance
    {
        public readonly AttendanceBLL _attendanceBLL;
        public AttendanceImpl(AttendanceBLL attendanceBLL)
        {
            _attendanceBLL = attendanceBLL;
        }

        public CommonResponse Attendance(AttendanceReqDTO attendanceReqDTO)
        {
            return _attendanceBLL.Attendance(attendanceReqDTO);
        }
    }
}
