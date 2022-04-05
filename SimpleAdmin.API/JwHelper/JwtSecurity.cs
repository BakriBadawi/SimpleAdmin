using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleAdmin.API.JwHelper;
public class JwtSecurity : IJwtSecurity
{
    private readonly JwSettings _jwSettings;

    public JwtSecurity(IOptions<JwSettings> jwSettings)
    {
        _jwSettings = jwSettings.Value;
    }
    public LoginResultModel generateJwtToken(User user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwSettings.Secret);
        DateTime expires = DateTime.UtcNow.AddDays(7);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("UserName", 
            user.FullName.ToString()) }),
            Expires = expires,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new LoginResultModel(tokenHandler.WriteToken(token), expires);
    }
    public int? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwSettings.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
        }
        catch
        {
            return null;
        }
    }
}

