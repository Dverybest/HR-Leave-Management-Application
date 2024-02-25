using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain;
using HRLeaveManagement.Persistence.DatabaseContext;
using HRLeaveManagement.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence;

public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
{
    public LeaveAllocationRepository(HrDatabaseContext context) : base(context)
    {
    }

    public async Task AddAllocations(List<LeaveAllocation> allocations)
    {
        await _context.AddRangeAsync(allocations);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
    {
        return await _context.LeaveAllocations
           .AnyAsync(q => q.LeaveTypeId == leaveTypeId
               && q.EmployeeId == userId
               && q.Period == period);
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
    {
        var leaveAllocations = await _context.LeaveAllocations
            .Include(q => q.LeaveType)
            .ToListAsync();
        return leaveAllocations;
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId)
    {
        var leaveAllocations = await _context.LeaveAllocations
           .Where(q => q.EmployeeId == userId)
           .Include(q => q.LeaveType)
           .ToListAsync();
        return leaveAllocations;
    }

    public async Task<LeaveAllocation?> GetLeaveAllocationWithDetails(int id)
    {
        var leaveAllocation = await _context.LeaveAllocations
             .FirstOrDefaultAsync(q => q.Id == id);
        return leaveAllocation;
    }

    public async Task<LeaveAllocation?> GetUserLeaveAllocations(string userId, int leaveTypeId)
    {
        var leaveAllocation = await _context.LeaveAllocations
            .FirstOrDefaultAsync(q => q.LeaveTypeId == leaveTypeId
                && q.EmployeeId == userId);

        return leaveAllocation;
    }
}
