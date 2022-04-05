namespace SimpleAdmin.Model;
public class UserModel
{
    private User user;
    public UserModel()
    {
        user = new User();
    }
    public UserModel(User initUser)
    {
        user = initUser;
    }
    [JsonIgnore]
    public User DBUser
    {
        get {return user; }
    }
    public int id
    {
        get { return user.Id; }
        set { user.Id = value; }
    }
    [Required]
    public string fullName
    {
        get { return user.FullName; }
        set { user.FullName = value; }
    }
    [Required]
    [EmailAddress]
    public string email
    {
        get { return user.Email; }
        set { user.Email = value; }
    }
    [Required]
    public string password
    {
        get { return user.Password; }
        set { user.Password = value; }
    }
    public string photo
    {
        get { return user.Photo; }
        set { user.Photo = value; }
    }
    [Required]
    public bool isEnabled
    {
        get { return user.IsEnabled; }
        set { user.IsEnabled = value; }
    }
}
