using AutoMapper;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;
using HRLeaveManagement.Domain;
using MediatR;


namespace HRLeaveManagement.Application;

public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, Unit>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly IUserService _userService;
    private readonly ILeaveTypeRepository _leaveTypeRepository;

    public CreateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, ILeaveTypeRepository leaveTypeRepository, IUserService userService)
    {
        _leaveTypeRepository = leaveTypeRepository;
        _leaveAllocationRepository = leaveAllocationRepository;
        _userService = userService;
    }

    public async Task<Unit> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveAllocationCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Count != 0)
            throw new BadRequestException("Invalid Leave Allocation Request", validationResult);

        //Get leave type
        var leaveType = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeId);

        //Get employee
        var employees = await _userService.GetEmployees();

        //Get period
        var period = DateTime.Now.Year;

        //Assign allocations if allocations doesn't for period and leave type;
        var allocations = new List<LeaveAllocation>();
        foreach (var employee in employees)
        {
            var allocationExist = await _leaveAllocationRepository.AllocationExists(employee.Id, request.LeaveTypeId, period);
            if (allocationExist == false)
            {
                allocations.Add(new LeaveAllocation
                {
                    EmployeeId = employee.Id,
                    LeaveTypeId = leaveType!.Id,
                    NumberOfDays = leaveType!.DefaultDays,
                    Period = period
                });
            }
        }
        if (allocations.Count!=0)
        {
            await _leaveAllocationRepository.AddAllocations(allocations);
        }
        return Unit.Value;
    }
}
