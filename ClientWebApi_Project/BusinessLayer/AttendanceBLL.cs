using DataLayer.Entities;
using DTO.Attendance;
using Helper.CommonModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class AttendanceBLL
    {
        public readonly ClientApiDbContext _db;

        public AttendanceBLL(ClientApiDbContext db)
        {
            _db = db;
        }

        public CommonResponse Attendance(AttendanceReqDTO attendanceReqDTO)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                AttendanceResDTO attendanceResDTO = new AttendanceResDTO();
                var clientDetail = _db.ClientMsts.FirstOrDefault(x => x.Id == attendanceReqDTO.Id );
                var leaveDetail = _db.LeaveMsts.FirstOrDefault(x => x.Id == attendanceReqDTO.Id);

                //var attendanceDetail = _db.AttendanceMsts.Where(x => x.DateAttendance == attendanceReqDTO.DateAttendance && x.Id == attendanceReqDTO.Id).ToList();
               // if (attendanceDetail.Count <= 0)
              //  {
                    if (clientDetail != null && leaveDetail != null)
                    {
                        AttendanceMst attendanceMst = new AttendanceMst();
                        attendanceMst.Id = attendanceReqDTO.Id;
                        attendanceMst.DateAttendance = attendanceReqDTO.DateAttendance;

                        if (attendanceReqDTO.TypesOfLeave.Trim() == LeaveType.Earned_Leave_Full_Day.ToString())
                        {
                            attendanceMst.TypesOfLeave = LeaveType.Earned_Leave_Full_Day.ToString();
                            leaveDetail.EarnedLeave = leaveDetail.EarnedLeave + 1;
                            attendanceMst.PresentAbsent = "Absent";
                            leaveDetail.ClosingLeaveBalance = leaveDetail.ClosingLeaveBalance - 1;
                        }
                        else if (attendanceReqDTO.TypesOfLeave.Trim() == LeaveType.Earned_Leave_Half_Day.ToString())
                        {
                            attendanceMst.TypesOfLeave = LeaveType.Earned_Leave_Half_Day.ToString();
                            leaveDetail.EarnedLeave = leaveDetail.EarnedLeave + Convert.ToDecimal(0.5);
                            //leaveDetail.TotalLeavesTaken = (decimal)(leaveDetail.TotalLeavesTaken + leaveDetail.EarnedLeave);
                            attendanceMst.PresentAbsent = "Half Day";
                            leaveDetail.ClosingLeaveBalance = leaveDetail.ClosingLeaveBalance - Convert.ToDecimal(0.5);
                        }
                        else if (attendanceReqDTO.TypesOfLeave.Trim() == LeaveType.Casual_Leave_Full_Day.ToString())
                        {
                            attendanceMst.TypesOfLeave = LeaveType.Casual_Leave_Full_Day.ToString();
                            leaveDetail.CasualLeave = leaveDetail.CasualLeave + 1;
                            attendanceMst.PresentAbsent = "Absent";
                            leaveDetail.ClosingLeaveBalance = leaveDetail.ClosingLeaveBalance - 1;
                        }
                        else if (attendanceReqDTO.TypesOfLeave.Trim() == LeaveType.Casual_Leave_Half_Day.ToString())
                        {
                            attendanceMst.TypesOfLeave = LeaveType.Casual_Leave_Half_Day.ToString();
                            leaveDetail.CasualLeave = leaveDetail.CasualLeave + Convert.ToDecimal(0.5);
                            attendanceMst.PresentAbsent = "Half Day";
                            leaveDetail.ClosingLeaveBalance = leaveDetail.ClosingLeaveBalance - Convert.ToDecimal(0.5);
                        }
                        else if (attendanceReqDTO.TypesOfLeave.Trim() == LeaveType.Seek_Leave_Full_Day.ToString())
                        {
                            attendanceMst.TypesOfLeave = LeaveType.Seek_Leave_Full_Day.ToString();
                            leaveDetail.SeekLeave = leaveDetail.SeekLeave + 1;
                            attendanceMst.PresentAbsent = "Absent";
                            leaveDetail.ClosingLeaveBalance = leaveDetail.ClosingLeaveBalance - 1;
                        }
                        else if (attendanceReqDTO.TypesOfLeave.Trim() == LeaveType.Seek_Leave_Half_Day.ToString())
                        {
                            attendanceMst.TypesOfLeave = LeaveType.Seek_Leave_Half_Day.ToString();
                            leaveDetail.SeekLeave = leaveDetail.SeekLeave + Convert.ToDecimal(0.5);
                            attendanceMst.PresentAbsent = "Half Day";
                            leaveDetail.ClosingLeaveBalance = leaveDetail.ClosingLeaveBalance - Convert.ToDecimal(0.5);
                        }
                        else if (attendanceReqDTO.TypesOfLeave.Trim() == LeaveType.Loss_Of_Pay_Leave_Full_Day.ToString())
                        {
                            attendanceMst.TypesOfLeave = LeaveType.Loss_Of_Pay_Leave_Full_Day.ToString();
                            leaveDetail.LossOfPayLeave = leaveDetail.LossOfPayLeave + 1;
                            attendanceMst.PresentAbsent = "Absent";
                           // leaveDetail.ClosingLeaveBalance = leaveDetail.ClosingLeaveBalance - 1;
                        }
                        else if (attendanceReqDTO.TypesOfLeave.Trim() == LeaveType.Loss_Of_Pay_Leave_Half_Day.ToString())
                        {
                            attendanceMst.TypesOfLeave = LeaveType.Loss_Of_Pay_Leave_Half_Day.ToString();
                            leaveDetail.LossOfPayLeave = leaveDetail.LossOfPayLeave + Convert.ToDecimal(0.5);
                            attendanceMst.PresentAbsent = "Half Day";
                            //leaveDetail.ClosingLeaveBalance = leaveDetail.ClosingLeaveBalance - Convert.ToDecimal(0.5);
                        }
                        else
                        {
                            attendanceMst.TypesOfLeave = "No Leave";
                            attendanceMst.PresentAbsent = "Present";
                        }

                        leaveDetail.TotalLeavesTaken = (decimal)(leaveDetail.EarnedLeave + leaveDetail.SeekLeave + leaveDetail.CasualLeave + leaveDetail.LossOfPayLeave);

                       // leaveDetail.ClosingLeaveBalance = (decimal)(leaveDetail.OpeningLeaveBalance - leaveDetail.TotalLeavesTaken) + (decimal)1.5; //right
                        //leaveDetail.ClosingLeaveBalance = (decimal)(leaveDetail.ClosingLeaveBalance - leaveDetail.TotalLeavesTaken);
                        leaveDetail.LeaveBalance = (decimal)(leaveDetail.ClosingLeaveBalance);


                        _db.Entry(leaveDetail).State = EntityState.Modified;
                        _db.AttendanceMsts.Add(attendanceMst);
                        _db.SaveChanges();


                        attendanceResDTO.Id = attendanceReqDTO.Id;
                        response.Data = attendanceResDTO;
                        response.Status = true;
                        response.Message = "added successfully!";
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Message = "user id is not exists";
                        response.StatusCode = System.Net.HttpStatusCode.BadGateway;
                    }
                //}
                //else
                //{
                //    response.Message = "Alreday added data for this date";
                //    response.StatusCode = System.Net.HttpStatusCode.BadGateway;
                //}
            }
            catch { throw; }
            return response;
        }
    }
}
