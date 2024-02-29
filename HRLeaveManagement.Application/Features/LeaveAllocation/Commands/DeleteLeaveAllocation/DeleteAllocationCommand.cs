using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation;

public class DeleteAllocationCommand:IRequest
{
    public int Id { get; set; }
}
