using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveType;

public record GetLeaveTypesQuery: IRequest<List<LeaveTypeDto>>;
