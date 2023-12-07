using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Helper.CommonModel
{
    public enum LeaveType
    {
        Earned_Leave_Full_Day,
        Earned_Leave_Half_Day,
        Casual_Leave_Full_Day,
        Casual_Leave_Half_Day,
        Seek_Leave_Full_Day,
        Seek_Leave_Half_Day,
        Loss_Of_Pay_Leave_Full_Day,
        Loss_Of_Pay_Leave_Half_Day
    }

    public class CommonResponse
    {
        public bool Status { get; set; } = false;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
        public string Message { get; set; } = "Something went wrong! Please try again.";
        public dynamic Data { get; set; } = null;
    }
}
