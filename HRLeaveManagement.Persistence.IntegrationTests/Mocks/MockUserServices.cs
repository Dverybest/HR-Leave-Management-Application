using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Models.Identity;
using Moq;

namespace HRLeaveManagement.Persistence.IntegrationTests.Mocks;

public class MockUserServices
{
    public static Mock<IUserService> GetMocktUserService()
    {
        var employees = new List<Employee>
            {
            new Employee
            {
                Id="5fee119a-b485-4ebc-83f1-01b57ef4665b",
                FirstName="Test2",
                LastName="Test2",
                Email="Test2@localhost.com"
            },
            new Employee
            {
                Id="b412e350-0e53-4bbd-9040-7d54ae0cc188",
                FirstName="Test",
                LastName="Test",
                Email="Test@localhost.com"
            }
        };
        var mockService = new Mock<IUserService>();

        var userId = "b412e350-0e53-4bbd-9040-7d54ae0cc188";

        mockService.Setup(s => s.UserId).Returns(userId);

        mockService.Setup(s => s.GetEmployee(userId))
            .ReturnsAsync(employees.FirstOrDefault(q => q.Id == userId)!);

        mockService.Setup(s => s.GetEmployees())
                   .ReturnsAsync(employees);

        return mockService;
    }
}
