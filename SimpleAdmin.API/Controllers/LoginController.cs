using SimpleAdmin.API.JwHelper;

namespace SimpleAdmin.API.Controllers;
[ApiController]
[Route("[controller]/[action]")]
[AutoLog]
public class LoginController : ControllerBase
{
    public DataAccess.Repository.IRepo<User, int> _repo { get; }
    public IJwtSecurity _jwtSecurity { get; }

    public LoginController(IRepo<User,int> repo, IJwtSecurity jwtSecurity)
    {
        _repo = repo;
        _jwtSecurity = jwtSecurity;
    }

    [HttpPost]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(string))]
    public IActionResult GetToken(LoginModel user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(user);
        }
        User dBUser=_repo.Get(c => c.Email == user.email).FirstOrDefault();
        if (dBUser==null || !BCrypt.Net.BCrypt.Verify(user.password, dBUser.Password))
        {
            return Unauthorized();
        }
        return Ok(_jwtSecurity.generateJwtToken(dBUser));
    }
}
