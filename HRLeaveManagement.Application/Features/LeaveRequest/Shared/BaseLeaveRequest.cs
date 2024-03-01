namespace HRLeaveManagement.Application.Features.LeaveRequest.Shared;

public abstract class BaseLeaveRequest
{
    public int LeaveTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
