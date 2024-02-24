using AutoMapper;
using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveType;
using HRLeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using HRLeaveManagement.Domain;

namespace HRLeaveManagement.Application.MappingProfiles;

public class LeaveTypeProfile:Profile
{
    public LeaveTypeProfile()
    {
        CreateMap<LeaveTypeDto,LeaveType>().ReverseMap();
        CreateMap<LeaveType,LeaveTypeDetailsDto>();
    }
}
