using System.Security.Claims;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Models.Identity;
using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HRLeaveManagement.Identity.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserService(UserManager<ApplicationUser> userManager,IHttpContextAccessor contextAccessor)
    {
        _userManager = userManager;
        _contextAccessor= contextAccessor;
    }

    public string UserId {get=> _contextAccessor.HttpContext?.User?.FindFirstValue("uid");}

    public async Task<Employee> GetEmployee(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if(user==null){
            throw new NotFoundException("User not found",userId);
        }
        return new Employee
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public async Task<List<Employee>> GetEmployees()
    {
        var employees = await _userManager.GetUsersInRoleAsync("Employee");
        return employees.Select(q => new Employee
        {
            Id = q.Id,
            Email = q.Email!,
            FirstName = q.FirstName,
            LastName = q.LastName
        }).ToList();
    }
}
