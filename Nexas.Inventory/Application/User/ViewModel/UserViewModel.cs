using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Nexas.Inventory.Application.User.ViewModel;
public class UserViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Password { get; set; }

    public UserViewModel() { }

    public UserViewModel(int id, string name, string email, string password)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
    }
}
