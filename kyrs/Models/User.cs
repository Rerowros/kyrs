using System.ComponentModel.DataAnnotations;
using BCrypt.Net;

namespace kyrs.Models;

public class User
{
    public int Id { get; set; }
    [MaxLength(10)] public string? Login { get; set; }
    [MaxLength(255)] public string? PasswordHash { get; set; }
    [MaxLength(20)] public string? Role { get; set; }

    public void SetPassword(string password)
    {
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool ValidatePassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }
}
