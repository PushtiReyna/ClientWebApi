using DTO.Salary;
using Helper.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface ISalary
    {
        public CommonResponse SalaryClient(SalaryClientReqDTO salaryClientReqDTO);

        public CommonResponse DownloadSalary(DownloadSalaryReqDTO downloadSalaryReqDTO);
    }
}
