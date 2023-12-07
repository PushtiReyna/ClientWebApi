using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class LeaveMst
{
    public int LeaveId { get; set; }

    public int Id { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public decimal OpeningLeaveBalance { get; set; }

    public decimal ClosingLeaveBalance { get; set; }

    public decimal? EarnedLeave { get; set; }

    public decimal? CasualLeave { get; set; }

    public decimal? SeekLeave { get; set; }

    public decimal TotalLeavesTaken { get; set; }

    public decimal LeaveBalance { get; set; }

    public decimal MonthLeave { get; set; }

    public decimal? LossOfPayLeave { get; set; }
}
