using DataLayer.Entities;
using DTO.ImportExcelData;
using Helper.CommonModel;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

//using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Bcpg;
using System;
using System.Data;
using System.IO;
using System.Xml;
//using Excel = Microsoft.Office.Interop.Excel;

namespace BusinessLayer
{
    public class ExcelBLL
    {
        public readonly ClientApiDbContext _db;
        public ExcelBLL(ClientApiDbContext db)
        {
            _db = db;
        }

        public CommonResponse ImportExcelData(GetDataReqDTO getDataReqDTO)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var stream = new MemoryStream())
                {
                    getDataReqDTO.ExcelFile.CopyTo(stream);
                    var leaveList = new List<LeaveMst>();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowcount = worksheet.Dimension.Rows;

                        for (int rowIterator = 4; rowIterator <= rowcount; rowIterator++)
                        {
                            LeaveMst leaveMst = new LeaveMst();

                            var clientDetail = _db.ClientMsts.FirstOrDefault(x => x.Fullname == worksheet.Cells[rowIterator, 2].Value.ToString());

                            if (clientDetail != null)
                            {
                                leaveMst.Id = clientDetail.Id;

                                leaveMst.Month = DateTime.Now.Month - 2;
                                leaveMst.Year = DateTime.Now.Year;

                                leaveMst.OpeningLeaveBalance = Convert.ToDecimal(worksheet.Cells[rowIterator, 55].Value);

                                if (worksheet.Cells[rowIterator, 51].Value == null)
                                {
                                    leaveMst.EarnedLeave = Convert.ToDecimal(0);
                                }
                                else
                                {
                                    leaveMst.EarnedLeave = Convert.ToDecimal(worksheet.Cells[rowIterator, 51].Value);
                                }
                                if (worksheet.Cells[rowIterator, 52].Value == null)
                                {
                                    leaveMst.CasualLeave = Convert.ToDecimal(0);
                                }
                                else
                                {
                                    leaveMst.CasualLeave = Convert.ToDecimal(worksheet.Cells[rowIterator, 52].Value);
                                }
                                if (worksheet.Cells[rowIterator, 53].Value == null)
                                {
                                    leaveMst.SeekLeave = Convert.ToDecimal(0);
                                }
                                else
                                {
                                    leaveMst.SeekLeave = Convert.ToDecimal(worksheet.Cells[rowIterator, 53].Value);
                                }
                                leaveMst.TotalLeavesTaken = Convert.ToDecimal(worksheet.Cells[rowIterator, 54].Value);
                                leaveMst.LeaveBalance = Convert.ToDecimal(worksheet.Cells[rowIterator, 55].Value);
                                leaveMst.ClosingLeaveBalance = leaveMst.LeaveBalance;
                                leaveMst.MonthLeave = Convert.ToDecimal(0);
                                leaveMst.LossOfPayLeave = Convert.ToDecimal(0);
                                leaveList.Add(leaveMst);
                            }
                            else
                            {
                                response.Message = "username not match!";
                                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                return response;
                            }
                        }
                        _db.LeaveMsts.AddRange(leaveList);
                        _db.SaveChanges();
                        response.Status = true;
                        response.Message = "success!";
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                    }
                    #region MyRegion

                    //    using (var package = new ExcelPackage(stream))
                    //    {
                    //         Get the worksheet
                    //        var worksheet = package.Workbook.Worksheets["Sheet3"];

                    //         Get the column index for the "OCT-2023" column
                    //        var octoberColumnIndex = worksheet.Cells["AY1"].FirstOrDefault(c => c.Value.ToString() == "OCT-2023").Start.Column;

                    //         Get the data for the "OCT-2023" column
                    //        var octoberData = worksheet.Cells[2, octoberColumnIndex, worksheet.Dimension.End.Row, octoberColumnIndex].Select(c => c.Value).ToList();

                    //         Map the header names to the database columns
                    //        var headerMap = new Dictionary<string, string>
                    //        {                       {"Name","Name"},
                    //                                { "EL", "EL" },
                    //                                { "CL","CL" },
                    //                                { "SL", "SL" },
                    //                                { "Total Leaves Taken", "TotalLeavesTaken" },
                    //                                { "Leave Balance", "LeaveBalance" }
                    //        };

                    //         Save the data to the database

                    //        for (int i = 0; i < octoberData.Count; i += 4)
                    //        {
                    //            LeaveMst leaveMst = new LeaveMst();
                    //            var clientDetail = _db.ClientMsts.FirstOrDefault(x => x.Fullname == worksheet.Cells["B"].Value.ToString());
                    //            if (clientDetail != null)
                    //            {
                    //                leaveMst.Id = clientDetail.Id;
                    //                if (octoberData[i + 1] == null)
                    //                {
                    //                    leaveMst.EarnedLeave = Convert.ToDecimal(0);
                    //                }
                    //                else
                    //                {
                    //                    leaveMst.EarnedLeave = Convert.ToDecimal(octoberData[i + 1]);
                    //                }
                    //                if (octoberData[i + 2] == null)
                    //                {
                    //                    leaveMst.CasualLeave = Convert.ToDecimal(0);
                    //                }
                    //                else
                    //                {
                    //                    leaveMst.CasualLeave = Convert.ToDecimal(octoberData[i + 2]);
                    //                }
                    //                if (octoberData[i + 3] == null)
                    //                {
                    //                    leaveMst.SeekLeave = Convert.ToDecimal(0);
                    //                }
                    //                else
                    //                {
                    //                    leaveMst.SeekLeave = Convert.ToDecimal(octoberData[i + 3]);
                    //                }
                    //                leaveMst.TotalLeavesTaken = Convert.ToDecimal(octoberData[i + 4]);
                    //                leaveMst.LeaveBalance = Convert.ToDecimal(octoberData[i + 5]);
                    //                leaveMst.OpeningLeaveBalance = Convert.ToDecimal(octoberData[i + 5]);
                    //                leaveMst.Month = DateTime.Now.Month - 2;
                    //                leaveMst.Year = DateTime.Now.Year;
                    //                leaveList.Add(leaveMst);
                    //            }
                    //            else
                    //            {
                    //                response.Message = "username not match!";
                    //                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    //                return response;
                    //            }
                    //        }
                    //        _db.LeaveMsts.AddRange(leaveList);
                    //        _db.SaveChanges();
                    //        response.Status = true;
                    //        response.Message = "success!";
                    //        response.StatusCode = System.Net.HttpStatusCode.OK;
                    //    } 
                    #endregion
                }
            }
            catch { throw; }
            return response;
        }

        public CommonResponse EarnedMonthlyLeave()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                //&& x.Month == DateTime.Now.Month - 1 && x.Year == DateTime.Now.Year

                var leaveDetailList = _db.LeaveMsts.Where(x => x.OpeningLeaveBalance >= 0 && x.MonthLeave == 0 ).ToList();
                var List2 = _db.LeaveMsts.Where( x => x.MonthLeave != 0 && (x.Month == DateTime.Now.Month - 1 || x.Month == DateTime.Now.Month - 2)).ToList();
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
                else
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
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
            }
            catch { throw; }
            return response;
        }
    }
}
