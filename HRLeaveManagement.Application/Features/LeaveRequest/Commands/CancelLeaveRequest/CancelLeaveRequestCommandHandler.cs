using System;
using HRLeaveManagement.Application.Contracts.Email;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Models.Email;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest
{
    public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand, Unit>
    {
        private readonly IEmailSender _emailSender;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IAppLogger<CancelLeaveRequestCommandHandler> _appLogger;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;

        public CancelLeaveRequestCommandHandler(IEmailSender emailSender, ILeaveRequestRepository leaveRequestRepository, IAppLogger<CancelLeaveRequestCommandHandler> appLogger,ILeaveAllocationRepository leaveAllocationRepository)
        {
            _emailSender = emailSender;
            _leaveRequestRepository = leaveRequestRepository;
            _appLogger = appLogger;
            _leaveAllocationRepository =  leaveAllocationRepository;
        }

        public async Task<Unit> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);
            if (leaveRequest == null)
                throw new NotFoundException(nameof(LeaveRequest), request.Id);

            if (leaveRequest.Cancelled)
                throw new BadRequestException("Leave request has already been canceled");

            leaveRequest.Cancelled = true;
            await _leaveRequestRepository.UpdateAsync(leaveRequest);

            if (leaveRequest.Approved == true)
            {
                int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                var allocation = await _leaveAllocationRepository.GetUserLeaveAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);
                allocation!.NumberOfDays += daysRequested;
                await _leaveAllocationRepository.UpdateAsync(allocation);
            }

            try
            {
                var email = new EmailMessage
                {
                    To = string.Empty,
                    Body = $"Your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D} "
                   + $"has been cancelled successfully.",
                    Subject = "Leave Request Cancelled"
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
}

