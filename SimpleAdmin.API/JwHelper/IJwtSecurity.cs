namespace SimpleAdmin.API.JwHelper;

public interface IJwtSecurity
{
    LoginResultModel generateJwtToken(User user);
    int? ValidateToken(string token);
}