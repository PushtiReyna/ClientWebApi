using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class AttendanceMst
{
    public int AttendanceId { get; set; }

    public int Id { get; set; }

    public DateTime DateAttendance { get; set; }

    public string TypesOfLeave { get; set; } = null!;

    public string PresentAbsent { get; set; } = null!;
}
