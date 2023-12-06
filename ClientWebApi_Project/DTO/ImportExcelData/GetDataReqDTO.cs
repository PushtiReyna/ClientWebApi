using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ImportExcelData
{
    public class GetDataReqDTO
    {
        public dynamic ExcelFile { get; set; }
    }
}
