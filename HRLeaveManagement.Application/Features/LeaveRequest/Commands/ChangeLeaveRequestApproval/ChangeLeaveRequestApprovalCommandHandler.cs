using AutoMapper;
using HRLeaveManagement.Application.Contracts.Email;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;
using HRLeaveManagement.Application.Models.Email;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval;

public class ChangeLeaveRequestApprovalCommandHandler : IRequestHandler<ChangeLeaveRequestApprovalCommand, Unit>
{
    private readonly IEmailSender _emailSender;
    private readonly IMapper _mapper;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IAppLogger<CreateLeaveRequestCommandHandler> _appLogger;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;

    public ChangeLeaveRequestApprovalCommandHandler(IEmailSender emailSender, IMapper mapper, ILeaveRequestRepository leaveRequestRepository, ILeaveTypeRepository leaveTypeRepository, IAppLogger<CreateLeaveRequestCommandHandler> appLogger,ILeaveAllocationRepository leaveAllocationRepository)
    {
        _emailSender = emailSender;
        _mapper = mapper;
        _leaveRequestRepository = leaveRequestRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _appLogger = appLogger;
        _leaveAllocationRepository = leaveAllocationRepository;
    }

    public async Task<Unit> Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

        if (leaveRequest is null)
            throw new NotFoundException(nameof(LeaveRequest), request.Id);

        leaveRequest.Approved = request.Approved;
        await _leaveRequestRepository.UpdateAsync(leaveRequest);

        if (leaveRequest.Approved==true)
        {
            int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
            var allocation = await _leaveAllocationRepository.GetUserLeaveAllocations(leaveRequest.RequestingEmployeeId,leaveRequest.LeaveTypeId);
            allocation!.NumberOfDays -= daysRequested;
            await _leaveAllocationRepository.UpdateAsync(allocation);
        }
        try
        {
            var email = new EmailMessage
            {
                To = string.Empty,
                Body = $"The approval status for your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D} "
               + $"has been updated.",
                Subject = "Leave Request Approval Status Update"
            };
            await _emailSender.SendEmail(email);
        }
        catch (Exception ex)
        {
            _appLogger.LogWarning(ex.Message);
        }

        return Unit.Value;
    }
}
