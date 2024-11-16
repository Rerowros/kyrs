using System.ComponentModel.DataAnnotations;
using BCrypt.Net;

namespace kyrs.Models;

public class User
{
    public int Id { get; set; }
    [MaxLength(10)] public string? Login { get; set; }
    [MaxLength(10)] public string? Password { get; set; }
    [MaxLength(150)] private string? PasswordHash { get; set; }

    public void SetPassword(string password)
    {
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool ValidatePassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }
}