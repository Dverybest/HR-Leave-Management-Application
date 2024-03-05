using System;
using System.ComponentModel.DataAnnotations;

namespace HRLeaveManagement.Application.Models.Identity;

public class Employee
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}

