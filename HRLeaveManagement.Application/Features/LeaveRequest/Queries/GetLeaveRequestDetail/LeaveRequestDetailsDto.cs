using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveType;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetail;

public class LeaveRequestDetailsDto
{
    public int Id { get; set; }
    public required LeaveTypeDto LeaveType { get; set; }
    public int LeaveTypeId { get; set; }
    public string RequestingEmployeeId { get; set; } = string.Empty;
    public DateTime DateRequested { get; set; }
    public DateTime DateActioned { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string RequestComments { get; set; } = string.Empty;
    public bool? Approved { get; set; }
    public bool Cancelled { get; set; }
}

