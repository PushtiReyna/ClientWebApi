using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Client
{
    public class UploadDocumentReqDTO
    {
        public string? Token { get; set; }
        public string? DocumentName { get; set; }
        public dynamic Document { get; set; }
    }
}
