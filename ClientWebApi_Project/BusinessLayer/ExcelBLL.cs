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

                            var clientMst = _db.ClientMsts.FirstOrDefault(x => x.Fullname == worksheet.Cells[rowIterator, 2].Value.ToString());

                            if (clientMst != null)
                            {

                                leaveMst.Id = clientMst.Id;

                                leaveMst.Month = DateTime.Now.Month;
                                leaveMst.Year = DateTime.Now.Year;

                                leaveMst.OpeningLeaveBalance = Convert.ToDecimal(worksheet.Cells[rowIterator, 60].Value);

                                if (worksheet.Cells[rowIterator, 56].Value == null)
                                {
                                    leaveMst.EarnedLeave = Convert.ToDecimal(0);
                                }
                                else
                                {
                                    leaveMst.EarnedLeave = Convert.ToDecimal(worksheet.Cells[rowIterator, 56].Value);
                                }
                                if (worksheet.Cells[rowIterator, 57].Value == null)
                                {
                                    leaveMst.CasualLeave = Convert.ToDecimal(0);
                                }
                                else
                                {
                                    leaveMst.CasualLeave = Convert.ToDecimal(worksheet.Cells[rowIterator, 57].Value);
                                }
                                if (worksheet.Cells[rowIterator, 58].Value == null)
                                {
                                    leaveMst.SeekLeave = Convert.ToDecimal(0);
                                }
                                else
                                {
                                    leaveMst.SeekLeave = Convert.ToDecimal(worksheet.Cells[rowIterator, 58].Value);
                                }
                                leaveMst.TotalLeavesTaken = Convert.ToDecimal(worksheet.Cells[rowIterator, 59].Value);
                                leaveMst.LeaveBalance = Convert.ToDecimal(worksheet.Cells[rowIterator, 60].Value);
                                leaveMst.ClosingLeaveBalance = leaveMst.LeaveBalance;
                                leaveMst.MonthLeave = Convert.ToDecimal(0);
                                //leaveList.Add(leaveMst);


                                _db.LeaveMsts.Add(leaveMst);
                                _db.SaveChanges();
                            }
                            else
                            {

                                response.Message = "username not match!";
                                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                            }
                        }

                        response.Status = true;
                        response.Message = "success!";
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                    }
                }

                #region MyRegion

                //Excel.Application excelApp = new Excel.Application();
                //Excel.Workbook workbook = excelApp.Workbooks.Open("C:\\Users\\Reyna\\Downloads\\2023_leave balance Sheet");
                //Excel.Worksheet worksheet = workbook.Sheets[1];
                //Excel.Range range = worksheet.Range["BC:BH"];

                //var leaveList = new List<LeaveMst>();
                //for (int i = 4; i <= range.Rows.Count; i++)
                //{
                //    LeaveMst leaveMst = new LeaveMst();

                //    leaveMst.OpeningLeaveBalance = Convert.ToDecimal(range.Rows[i, 1].Value);
                //    if (range.Cells[i, 2].Value == null)
                //    {
                //        leaveMst.EarnedLeave = Convert.ToDecimal(0);
                //    }
                //    else
                //    {
                //        leaveMst.EarnedLeave = Convert.ToDecimal(range.Cells[i, 2].Value);
                //    }
                //    if (range.Cells[i, 3].value == null)
                //    {
                //        leaveMst.CasualLeave = Convert.ToDecimal(0);
                //    }
                //    else
                //    {
                //        leaveMst.CasualLeave = Convert.ToDecimal(range.Cells[i, 3].Value);
                //    }
                //    if (range.Cells[i, 4].value == null)
                //    {
                //        leaveMst.SeekLeave = Convert.ToDecimal(0);
                //    }
                //    else
                //    {
                //        leaveMst.SeekLeave = Convert.ToDecimal(range.Cells[i, 4].Value);
                //    }
                //    leaveMst.TotalLeavesTaken = Convert.ToDecimal(range.Cells[i, 5].value);
                //    leaveMst.LeaveBalance = Convert.ToDecimal(range.Cells[i, 6].value);
                //    leaveMst.ClosingLeaveBalance = leaveMst.LeaveBalance;
                //    leaveMst.MonthLeave = Convert.ToDecimal(1.5);

                //    //leaveMst.EarnedLeave = Convert.ToDecimal(range.Cells[i, 2].Value);
                //    //leaveMst.OpeningLeaveBalance = range.Rows[i, 1].Value;
                //    //leaveMst.EarnedLeave = range.Cells[i, 2].Value.ToString();
                //    //leaveMst.CasualLeave = range.Cells[i, 3].value.ToString();
                //    //leaveMst.SeekLeave = range.Cells[i, 4].value.ToString();
                //    //leaveMst.TotalLeavesTaken = range.Cells[i, 5].value.ToString();
                //    //leaveMst.ClosingLeaveBalance = range.Cells[i, 6].value.ToString();
                //    //leaveMst.Month = range.Cells[i, 4].value.ToString();
                //    //leaveMst.Year = range.Cells[i, 5].value.ToString();
                //    //leaveMst.MonthLeave = (decimal)1.5;
                //    //leaveMst.ClosingLeaveBalance = range.Cells[i, 6].value.ToString();

                //    leaveList.Add(leaveMst);
                //}
                //_db.LeaveMsts.AddRange(leaveList);
                //_db.SaveChanges(); 
                #endregion


                #region MyRegion
                //using (var stream = new MemoryStream())
                //{
                //    getDataReqDTO.ExcelFile.CopyTo(stream);
                //    var leaveList = new List<LeaveMst>();
                //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                //    using (var package = new ExcelPackage(stream))
                //    {
                //        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                //        var range = worksheet.SelectedRange["BC:BH"];
                //        var row = worksheet.Rows[4];

                //        foreach (var cell in range)
                //        {
                //            LeaveMst leaveMst = new LeaveMst();

                //            leaveMst.OpeningLeaveBalance = Convert.ToDecimal(range.Rows[row, 1].Value);
                //            if (range.Cells[i, 2].Value == null)
                //            {
                //                leaveMst.EarnedLeave = Convert.ToDecimal(0);
                //            }
                //            else
                //            {
                //                leaveMst.EarnedLeave = Convert.ToDecimal(range.Cells[i, 2].Value);
                //            }
                //        }
                //    }
                //} 
                #endregion
            }
            catch { throw; }
            return response;
        }

        public CommonResponse EarnedMonthlyLeave()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var leaveMsts = _db.LeaveMsts.Where(x => x.OpeningLeaveBalance >= 0 && x.MonthLeave == 0 && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year).ToList();
                if(leaveMsts.Count > 0)
                {
                    foreach (var employee in leaveMsts)
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
                    response.Message = "Data are already updated";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
            }
            catch { throw; }
            return response;
        }
    }
}
