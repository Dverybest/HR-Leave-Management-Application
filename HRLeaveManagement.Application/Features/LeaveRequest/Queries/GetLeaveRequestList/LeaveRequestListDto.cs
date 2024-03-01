using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveType;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;

public class LeaveRequestListDto
{
    public required LeaveTypeDto LeaveType { get; set; }
    public string RequestingEmployeeId { get; set; } = string.Empty;
    public DateTime DateRequested { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool? Approved { get; set; }
}
