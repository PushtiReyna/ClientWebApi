using System.ComponentModel.DataAnnotations;

namespace ClientWebApi.ViewModel.Attendance
{
    public class AttendanceReqViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateAttendance { get; set; }

        public string TypesOfLeave { get; set; } = null!;
    }
}
