namespace Nexas.Inventory.Application.Auth.ViewModel;
public class AuthResponseViewModel
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public AuthUserViewModel User { get; set; }
}

