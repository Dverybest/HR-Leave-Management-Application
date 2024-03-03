using HRLeaveManagement.Domain;
using HRLeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Shouldly;

namespace HRLeaveManagement.Persistence.IntegrationTests;

public class HrDatabaseContextTests
{
    private HrDatabaseContext _hrDatabaseContext;

    public HrDatabaseContextTests()
    {
        var dbOptions = new DbContextOptionsBuilder<HrDatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        _hrDatabaseContext = new HrDatabaseContext(dbOptions);
    }
    [Fact]
    public async void Save_SetDateCreatedValue()
    {
        //Arrange
        var leaveType = new LeaveType()
        {
            Id = 1,
            DefaultDays = 10,
            Name = "Test Vacation"
        };

        //Act
        await _hrDatabaseContext.LeaveTypes.AddAsync(leaveType);
        await _hrDatabaseContext.SaveChangesAsync();

        //Assert
        leaveType.DateCreated.ShouldNotBeNull();
    }

    [Fact]
    public async void Save_SetDateModifiedValue()
    {
        var leaveType = new LeaveType()
        {
            Id = 1,
            DefaultDays = 10,
            Name = "Test Vacation"
        };
        //Act
        await _hrDatabaseContext.LeaveTypes.AddAsync(leaveType);
        await _hrDatabaseContext.SaveChangesAsync();

        //Assert
        leaveType.DateCreated.ShouldNotBeNull();
    }
}