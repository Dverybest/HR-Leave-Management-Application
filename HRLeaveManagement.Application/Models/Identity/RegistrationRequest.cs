using System;
using System.ComponentModel.DataAnnotations;

namespace HRLeaveManagement.Application.Models.Identity;

public class RegistrationRequest
{
    [Required]
    public required string FirstName { get; set; }
    [Required]
    public required string LastName { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string UserName { get; set; }
    [Required]
    public required string Password { get; set; }
}

