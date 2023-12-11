using DataLayer.Entities;
using Helper.CommonModel;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class SchedulerBLL : IJob
    {
        public readonly ClientApiDbContext _db;
        public SchedulerBLL(ClientApiDbContext db)
        {
            _db = db;
        }

        public async Task<CommonResponse> EarnedMonthlyLeave()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                //&& x.Month == DateTime.Now.Month - 1 && x.Year == DateTime.Now.Year

                var leaveDetailList = _db.LeaveMsts.Where(x => x.OpeningLeaveBalance >= 0 && x.MonthLeave == 0).ToList();
                var List2 = _db.LeaveMsts.Where(x => x.MonthLeave != 0 && (x.Month == DateTime.Now.Month - 2 /*|| x.Month == DateTime.Now.Month - 2*/)).ToList();
                if (leaveDetailList.Count > 0)
                {
                    var leaveList = new List<LeaveMst>();
                    foreach (var employee in leaveDetailList)
                    {
                        employee.MonthLeave = Convert.ToDecimal(1.5);
                        employee.ClosingLeaveBalance = employee.ClosingLeaveBalance + employee.MonthLeave;
                        employee.LeaveBalance = employee.ClosingLeaveBalance;
                        _db.Entry(employee).State = EntityState.Modified;
                        _db.SaveChanges();

                    }
                    response.Status = true;
                    response.Message = "updated successfully!";
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                }
                else if (List2.Count > 0)
                {
                    var leaveList = new List<LeaveMst>();
                    foreach (var employee in List2)
                    {
                        LeaveMst leaveMst = new LeaveMst();
                        leaveMst.Month = employee.Month + 1;
                        leaveMst.Year = employee.Year;
                        leaveMst.Id = employee.Id;
                        leaveMst.OpeningLeaveBalance = employee.ClosingLeaveBalance;
                        leaveMst.EarnedLeave = 0;
                        leaveMst.SeekLeave = 0;
                        leaveMst.CasualLeave = 0;
                        leaveMst.LossOfPayLeave = 0;
                        leaveMst.ClosingLeaveBalance = employee.ClosingLeaveBalance;
                        leaveMst.LeaveBalance = leaveMst.ClosingLeaveBalance;
                        leaveList.Add(leaveMst);
                    }
                    _db.LeaveMsts.AddRange(leaveList);
                    _db.SaveChanges();
                    response.Message = "new data added";
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "not data updated not new data added";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
            }
            catch { throw; }
            return response;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return EarnedMonthlyLeave();
        }
    }
}
