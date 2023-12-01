using DataLayer.Entities;
using DTO.Salary;
using Helper.CommonModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class SalaryBLL
    {
        public readonly ClientApiDbContext _db;
        public SalaryBLL(ClientApiDbContext db)
        {
            _db = db;
        }

        public CommonResponse SalaryClient(SalaryClientReqDTO salaryClientReqDTO)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                SalaryClientResDTO salaryClientResDTO = new SalaryClientResDTO();

                var clientDetail = _db.ClientMsts.FirstOrDefault(x => x.Id == salaryClientReqDTO.Id && x.IsDelete == false);

                if (clientDetail != null)
                {
                    SalaryMst salaryMst = new SalaryMst();
                    salaryMst.Id = salaryClientReqDTO.Id;
                    // salaryMst.Month = salaryClientReqDTO.Month.Month;
                    // salaryMst.Year = salaryClientReqDTO.Month.Year;

                    salaryMst.Month = salaryClientReqDTO.Month;
                    salaryMst.Year = salaryClientReqDTO.Year;

                    salaryMst.FixedBasicSalary = (decimal)(clientDetail.Ctc) * ((decimal)(40.0 / 100.0));

                    salaryMst.FixedHra = salaryMst.FixedBasicSalary * ((decimal)0.4);

                    salaryMst.FixedConveyanceAllowance = 1600;

                    salaryMst.FixedMedicalAllowance = 1250;

                    salaryMst.AdditionalHraAllowance = 2400;

                    salaryMst.TotalDays = 29;

                    salaryMst.PayableDays = salaryMst.TotalDays;

                    var PayableDays_TotalDays = salaryMst.PayableDays / salaryMst.TotalDays;

                    salaryMst.GrossSalaryPayable = ((decimal)(clientDetail.Ctc) * PayableDays_TotalDays);

                    salaryMst.Basic = (salaryMst.FixedBasicSalary * PayableDays_TotalDays);

                    var GrossSalaryPayable_Basic = (salaryMst.GrossSalaryPayable - salaryMst.Basic);

                    var FixedHra_PayableDays_TotalDays = (salaryMst.FixedHra * PayableDays_TotalDays);

                    var basic = ((decimal)0.4) * salaryMst.Basic;

                    // HouseRentAllowance = IF((O7-P7)<0.4*P7,O7-P7,G7*N7/K7)
                    if (GrossSalaryPayable_Basic < basic)
                    {
                        salaryMst.HouseRentAllowance = GrossSalaryPayable_Basic;
                    }
                    else
                    {
                        salaryMst.HouseRentAllowance = FixedHra_PayableDays_TotalDays;
                    }
                    //Pf = IF((F7)<15001,P7*12%,0)
                    if (salaryMst.FixedBasicSalary < 15001)
                    {
                        salaryMst.PfOne = salaryMst.Basic * ((decimal)0.12);
                    }
                    else
                    {
                        salaryMst.PfOne = 0;
                    }

                    salaryMst.PfTwo = salaryMst.PfOne;

                    salaryMst.EmployerContPf = salaryMst.PfTwo;

                    var HouseRentAllowance_EmployerContPf = salaryMst.HouseRentAllowance - salaryMst.EmployerContPf;

                    //ConveyanceAllowance = IF((O7-P7-Q7-R7)<1600*N7/K7,(O7-P7-Q7-R7),H7*N7/K7)
                    if ((GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf) < 1600 * PayableDays_TotalDays)
                    {
                        salaryMst.ConveyanceAllowance = GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf;
                    }
                    else
                    {
                        salaryMst.ConveyanceAllowance = salaryMst.FixedConveyanceAllowance * PayableDays_TotalDays;
                    }

                    // MedicalAllowance = IF((O7-P7-Q7-R7-S7)<1250*N7/K7,O7-P7-Q7-R7-S7,I7*N7/K7)
                    if ((GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf - salaryMst.ConveyanceAllowance) < 1250 * PayableDays_TotalDays)
                    {
                        salaryMst.MedicalAllowance = GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf - salaryMst.ConveyanceAllowance;
                    }
                    else
                    {
                        salaryMst.MedicalAllowance = salaryMst.FixedMedicalAllowance * PayableDays_TotalDays; ;
                    }

                    // Additional HRA Allowance =  IF((O7 - P7 - Q7 - R7 - S7 - T7) < 2400 * N7 / K7, O7 - P7 - Q7 - R7 - S7 - T7, J7 * N7 / K7)
                    if ((GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf - salaryMst.ConveyanceAllowance - salaryMst.MedicalAllowance) < 2400 * PayableDays_TotalDays)
                    {
                        salaryMst.SalaryAdditionalHraAllowance = GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf - salaryMst.ConveyanceAllowance - salaryMst.MedicalAllowance;
                    }
                    else
                    {
                        salaryMst.SalaryAdditionalHraAllowance = salaryMst.AdditionalHraAllowance * PayableDays_TotalDays;
                    }

                    //FlexibleAllowance=O7-SUM(P7:U7) //O7 - (P7 + Q7 + R7 + S7 + T7 + U7)
                    salaryMst.FlexibleAllowance = salaryMst.GrossSalaryPayable - (salaryMst.Basic + salaryMst.HouseRentAllowance + salaryMst.EmployerContPf + salaryMst.ConveyanceAllowance + salaryMst.MedicalAllowance + salaryMst.SalaryAdditionalHraAllowance);

                    salaryMst.Incentives = 0;


                    // Total = SUM(P7:V7)+W7 // P7 + Q7 + R7 + S7 + T7 + U7 + V7 + W7
                    salaryMst.Total = (decimal)(salaryMst.Basic + salaryMst.HouseRentAllowance + salaryMst.EmployerContPf + salaryMst.ConveyanceAllowance + salaryMst.MedicalAllowance + salaryMst.SalaryAdditionalHraAllowance + salaryMst.FlexibleAllowance + salaryMst.Incentives);

                    salaryMst.Esic = 0;

                    //Pt = =IF(X7<6000,0,IF(X7<9000,80,IF(X7<12000,150,IF(X7>=12000,200))))
                    if (salaryMst.Total < 6000)
                    {
                        salaryMst.Pt = 0;
                    }
                    else if (salaryMst.Total < 9000)
                    {
                        salaryMst.Pt = 80;
                    }
                    else if (salaryMst.Total < 12000)
                    {
                        salaryMst.Pt = 150;
                    }
                    else
                    {
                        salaryMst.Pt = 200;
                    }

                    salaryMst.Advances = 0;
                    salaryMst.IncomeTax = 0;

                    //=SUM(Y7:AE7) Y7 + Z7 + AA7 + AB7 + AC7 + AD7 + AE7

                    salaryMst.TotalDed = salaryMst.PfOne + salaryMst.PfTwo + salaryMst.Pt;

                    // ActualNetPayable X7-AF7
                    salaryMst.ActualNetPayable = salaryMst.Total - salaryMst.TotalDed;

                    salaryMst.IsActive = true;
                    salaryMst.CreatedBy = 1;
                    salaryMst.CreatedOn = DateTime.Now;

                    _db.SalaryMsts.Add(salaryMst);
                    _db.SaveChanges();

                    salaryClientResDTO.Salaryid = salaryMst.Salaryid;

                    response.Data = salaryClientResDTO;
                    response.Status = true;
                    response.Message = "Data added successfully";
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "client id is not valid";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
            }
            catch { throw; }
            return response;
        }
    }
}
