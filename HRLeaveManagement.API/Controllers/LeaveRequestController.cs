using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetail;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HRLeaveManagement.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class LeaveRequestController : Controller
{
    private readonly IMediator _mediator;

    public LeaveRequestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<LeaveRequestListDto>>> Get()
    {
        var leaveRequest = await _mediator.Send(new GetLeaveRequestListQuery());
        return Ok(leaveRequest);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveRequestDetailsDto>> Get(int id)
    {
        var leaveRequest = await _mediator.Send(new GetLeaveRequestDetailQuery { Id = id });
        return Ok(leaveRequest);
    }


    [HttpPost]
    public async Task<ActionResult> Post([FromBody]CreateLeaveRequestCommand leaveRequest)
    {
        await _mediator.Send(leaveRequest);
        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Put(UpdateLeaveRequestCommand updateLeaveRequestCommand)
    {
        await _mediator.Send(updateLeaveRequestCommand);
        return NoContent();
    }


    [HttpPatch]
    [Route("CancelRequest")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CancelRequest(CancelLeaveRequestCommand cancelLeaveRequestCommand)
    {
        await _mediator.Send(cancelLeaveRequestCommand);
        return NoContent();
    }

    [HttpPatch]
    [Route("UpdateApproval")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateApproval(ChangeLeaveRequestApprovalCommand changeLeaveRequestApprovalCommand)
    {
        await _mediator.Send(changeLeaveRequestApprovalCommand);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var deleteLeaveRequestCommand = new DeleteLeaveRequestCommand { Id = id };
        await _mediator.Send(deleteLeaveRequestCommand);
        return NoContent();
    }
}


