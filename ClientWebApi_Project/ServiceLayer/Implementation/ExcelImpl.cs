using BusinessLayer;
using DTO.ImportExcelData;
using Helper.CommonModel;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Implementation
{
    public class ExcelImpl : IExcel
    {
        public readonly ExcelBLL _excelBLL;
        public ExcelImpl(ExcelBLL excelBLL)
        {
            _excelBLL = excelBLL;
        }
        public CommonResponse ImportExcelData(GetDataReqDTO getDataReqDTO)
        {
            return _excelBLL.ImportExcelData(getDataReqDTO);
        }

        public CommonResponse EarnedMonthlyLeave()
        {
            return _excelBLL.EarnedMonthlyLeave();
        }
    }
}
