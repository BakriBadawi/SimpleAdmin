using SimpleAdmin.API.JwHelper;
using Microsoft.Extensions.Options;


namespace SimpleAdmin.API.XunitTest;

public class LoginControllerTest
{
    UserRepo repo;
    LoginController controller;
    public LoginControllerTest()
    {
        string dbName = Guid.NewGuid().ToString();
        DbFixture dbFixture = new DbFixture(dbName);
        repo = new UserRepo(dbFixture.DBOption);
        dbFixture.AddDummyData(repo.Context);
        JwSettings jwSettings = new JwSettings();
        jwSettings.Secret = "OPIk098$iwpoeqpwewqq9012$";
        IOptions<JwSettings> jwOption = Options.Create<JwSettings>(jwSettings);

        controller = new LoginController(repo, new JwtSecurity(jwOption));
    }
    [Fact]
    public void GetToken_WithCorrectCredentialShouldRetrunToken()
    {
        LoginModel loginModel = new LoginModel()
        {
            password = "123456",
            email = "Eamil@domian.com"
        };
        OkObjectResult tokenresult = (OkObjectResult)controller.GetToken(loginModel);
      
        Assert.NotNull (tokenresult.Value);
        
    }
    [Fact]
    public void GetToken_WithCorrectCredentialShouldRetrunUnauthorized()
    {
        LoginModel loginModel = new LoginModel()
        {
            password = "1234566",
            email = "Eamil@domian.com"
        };
        UnauthorizedResult tokenresult = (UnauthorizedResult)controller.GetToken(loginModel);

        Assert.NotNull(tokenresult);

    }
}
