using HRLeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;
using HRLeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType;
using HRLeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;
using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveType;
using HRLeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRLeaveManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LeaveTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<LeaveTypeDto>>> Get()
    {
        var leaveTypes = await _mediator.Send(new GetLeaveTypesQuery());
        return leaveTypes;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveTypeDetailsDto>> Get(int id)
    {
        var leaveType = await _mediator.Send(new GetLeaveTypeDetailsQuery(id));
        return Ok(leaveType);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post(CreateLeaveTypeCommand createLeaveType)
    {
        var response = await _mediator.Send(createLeaveType);
        return CreatedAtAction(nameof(Get), new { Id = response });
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Put(UpdateLeaveTypeCommand leaveType)
    {
        await _mediator.Send(leaveType);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteLeaveTypeCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
