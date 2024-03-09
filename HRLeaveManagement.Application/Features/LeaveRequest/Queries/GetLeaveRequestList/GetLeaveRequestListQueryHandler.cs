using AutoMapper;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;

public class GetLeaveRequestListQueryHandler : IRequestHandler<GetLeaveRequestListQuery, List<LeaveRequestListDto>>
{
    private readonly IMapper _mapper;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IUserService _userService;

    public GetLeaveRequestListQueryHandler(IMapper mapper, ILeaveRequestRepository leaveRequestRepository, IUserService userService)
    {
        _mapper = mapper;
        _leaveRequestRepository = leaveRequestRepository;
        _userService = userService;
    }
    public async Task<List<LeaveRequestListDto>> Handle(GetLeaveRequestListQuery request, CancellationToken cancellationToken)
    {
        List<Domain.LeaveRequest> leaveRequests;
        List<LeaveRequestListDto> leaveRequestsDto;
        if (request.IsLoggedInUser)
        {
            var employeeId = _userService.UserId;
            leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails(employeeId);
            var employee = await _userService.GetEmployee(employeeId);
            leaveRequestsDto = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
            foreach (var req in leaveRequestsDto)
            {
                req.Employee = employee;
            }
        }
        else
        {
            leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails();
            leaveRequestsDto = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
            foreach (var req in leaveRequestsDto)
            {
                var employee = await _userService.GetEmployee(req.RequestingEmployeeId);
                req.Employee = employee;
            }
        }
        return leaveRequestsDto;
    }
}
