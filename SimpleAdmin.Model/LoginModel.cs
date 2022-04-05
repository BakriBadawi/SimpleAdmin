namespace SimpleAdmin.Model;
public class LoginModel
{
    [Required]
    [EmailAddress]
    public string email { set; get; }
    [Required]
    public string password { set; get; }
}