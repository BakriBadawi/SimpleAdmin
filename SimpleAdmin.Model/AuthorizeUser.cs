namespace SimpleAdmin.Model;
public class AuthorizeUser {
    public bool IsAutorizes { set; get; } = false;
    public UserModel? User { set; get; } = null;
}
