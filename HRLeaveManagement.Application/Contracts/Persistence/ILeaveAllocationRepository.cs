using HRLeaveManagement.Domain;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation>
{
    Task<LeaveAllocation?> GetLeaveAllocationWithDetails(int id);
    Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails();
    Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId);
    Task<bool> AllocationExists(string userId, int leaveTypeId, int period);
    Task AddAllocations(List<LeaveAllocation> allocations);
    Task<LeaveAllocation?> GetUserLeaveAllocations(string userId, int leaveTypeId);
}
