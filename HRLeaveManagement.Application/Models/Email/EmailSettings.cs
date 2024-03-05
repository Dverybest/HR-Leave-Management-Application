namespace HRLeaveManagement.Application.Models.Email;

public class EmailSettings
{
    public string FormAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
}