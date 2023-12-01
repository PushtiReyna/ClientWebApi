using BusinessLayer;
using DTO.Salary;
using Helper.CommonModel;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Implementation
{
    public class SalaryImpl : ISalary
    {
        public readonly SalaryBLL _salaryBLL;
        public SalaryImpl(SalaryBLL salaryBLL)
        {
            _salaryBLL = salaryBLL;
        }

        public CommonResponse SalaryClient(SalaryClientReqDTO salaryClientReqDTO)
        {
            return _salaryBLL.SalaryClient(salaryClientReqDTO);
        }
    }
}
