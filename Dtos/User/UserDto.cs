using Charity.Models;

public class UserDto
{
    public Guid Id { get; set; }
    public string? FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PasswordHash { get; set; } = null!;
    public string Role { get; set; } = "Donor"; // Donor, Admin


}