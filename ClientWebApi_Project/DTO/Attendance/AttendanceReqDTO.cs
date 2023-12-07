using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Attendance
{
    public class AttendanceReqDTO
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateAttendance { get; set; }

        public string TypesOfLeave { get; set; } = null!;
    }
}
