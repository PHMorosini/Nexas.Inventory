using BCrypt.Net;
using Microsoft.AspNetCore.Identity;

namespace Nexas.Inventory.Domain.User.Entity
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }


        public void SetEmail(string email)
        {
            Email = email;
        }

        public void SetPassword(string password)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

        }
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        public UserEntity() { }

        public UserEntity(int id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            SetEmail(email);
            SetPassword(password);
        }
    }
}
