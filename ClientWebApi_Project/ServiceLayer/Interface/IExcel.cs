using DTO.ImportExcelData;
using Helper.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IExcel
    {
        public CommonResponse ImportExcelData(GetDataReqDTO getDataReqDTO);

        public CommonResponse EarnedMonthlyLeave();
    }
}
