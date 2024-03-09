using AutoMapper;
using HRLeaveManagement.Application.Contracts.Email;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using HRLeaveManagement.Application.Models.Email;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;

public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
{
    private readonly IAppLogger<CreateLeaveRequestCommandHandler> _appLogger;
    private readonly IEmailSender _emailSender;
    private readonly IMapper _mapper;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly IUserService _userService;

    public CreateLeaveRequestCommandHandler(IEmailSender emailSender, IMapper mapper, ILeaveRequestRepository leaveRequestRepository, ILeaveTypeRepository leaveTypeRepository, IAppLogger<CreateLeaveRequestCommandHandler> appLogger, IUserService userService, ILeaveAllocationRepository leaveAllocationRepository)
    {
        _appLogger = appLogger;
        _emailSender = emailSender;
        _mapper = mapper;
        _leaveRequestRepository = leaveRequestRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _userService = userService;
        _leaveAllocationRepository = leaveAllocationRepository;
    }
    public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveRequestCommandValidation(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.Errors.Count != 0)
            throw new BadRequestException("Invalid Leave Request", validationResult);

        var employeeId = _userService.UserId;
        var leaveAllocation = await _leaveAllocationRepository.GetUserLeaveAllocations(employeeId, request.LeaveTypeId);

        if (leaveAllocation == null)
        {
            validationResult.Errors.Add(
                new FluentValidation.Results.ValidationFailure(nameof(request.LeaveTypeId),
                    "You do not have any allocation for this leave type")
                );
            throw new BadRequestException("Invalid Leave Request", validationResult);
        }

        int daysRequested = (int)(request.EndDate - request.StartDate).TotalDays;
        if(daysRequested>leaveAllocation.NumberOfDays)
        {
            validationResult.Errors.Add(
               new FluentValidation.Results.ValidationFailure(nameof(request.LeaveTypeId),
                   "You do not have enough days for this request")
               );
            throw new BadRequestException("Invalid Leave Request", validationResult);
        }

        var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request);
        leaveRequest.RequestingEmployeeId = employeeId;
        leaveRequest.DateRequested = DateTime.Now;

        await _leaveRequestRepository.CreateAsync(leaveRequest);

        try
        {
            var email = new EmailMessage
            {
                To = string.Empty,
                Body = $"Your leave request for {request.StartDate} to {request.EndDate} "
               + $"has been updated successfully.",
                Subject = "Leave Request Submitted"
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
