using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Nexas.Inventory.Domain.User.Entity;

public class UserEntity
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public UserEntity() { }

    public UserEntity(int id, string name, string email, string? password)
    {
        Id = id;
        Name = name?.ToUpper() ?? throw new ArgumentNullException(nameof(name));
        SetEmail(email);

        if (!string.IsNullOrWhiteSpace(password))
            SetPassword(password);
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        Email = email.Trim();
    }

    public void SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }

    public void ChangePassword(string newPassword)
    {
        SetPassword(newPassword);
    }

    public void Edit(UserEntity user)
    {
        if (user == null)
                throw new ArgumentNullException(nameof(user));

        Id = user.Id;
        Name = user.Name?.ToUpper() ?? Name;
        SetEmail(user.Email);
    }
}

