using System;
namespace HRLeaveManagement.Application.Models.Identity;

public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Aduience { get; set; } = string.Empty;
    public double DurationInMinutes { get; set; }
}

