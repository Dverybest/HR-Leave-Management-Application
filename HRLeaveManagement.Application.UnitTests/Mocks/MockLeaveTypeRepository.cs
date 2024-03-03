using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain;
using Moq;

namespace HRLeaveManagement.Application.UnitTests.Mocks;

public class MockLeaveTypeRepository
{
    public static Mock<ILeaveTypeRepository> GeMocktLeaveTypeRepository()
    {
        var leaveTypes = new List<LeaveType>
        {
            new LeaveType{
                Id=1,
                DefaultDays=10,
                Name="Test Vacation"
            },
             new LeaveType{
                Id=2,
                DefaultDays=10,
                Name="Test Sick"
            },
             new LeaveType{
                Id=3,
                DefaultDays=10,
                Name="Test Maternity"
            },
        };
        var mockRepo = new Mock<ILeaveTypeRepository>();

        mockRepo.Setup(r => r.GetAsync()).ReturnsAsync(leaveTypes);

        mockRepo.Setup(r => r.CreateAsync(It.IsAny<LeaveType>()))
            .Returns((LeaveType leaveType) =>
            {
                leaveTypes.Add(leaveType);
                return Task.FromResult<LeaveType>(leaveType);
            });

        return mockRepo;
    }
}
