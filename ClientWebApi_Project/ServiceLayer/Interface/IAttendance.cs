﻿using DTO.Attendance;
using Helper.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IAttendance
    {
        public CommonResponse Attendance(AttendanceReqDTO attendanceReqDTO);
    }
}