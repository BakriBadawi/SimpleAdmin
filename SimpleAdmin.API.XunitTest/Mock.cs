using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SimpleAdmin.API.JwHelper;
using System.IO;


namespace SimpleAdmin.API.XunitTest
{
    internal static class Mock
    {
        //initate context with authorized user
        internal static void AuthorizedAccess(DbFixture dbFixture, string dbName) 
        {
            AuthorizeAttribute attr = new AuthorizeAttribute();
            ActionContext mockActionContext = new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor(),
            };

            JwSettings jwSettings = new JwSettings();
            jwSettings.Secret = "OPIk098$iwpoeqpwewqq9012$";
            IOptions<JwSettings> jwOption = Options.Create<JwSettings>(jwSettings);


            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", false)
               .Build();

            ServiceCollection services = new ServiceCollection();


            services.AddTransient<IRepo<User, int>, UserRepo>();
            services.AddTransient<IRepo<Item, Guid>, ItemRepo>();
            services.AddTransient<IRepo<Category, int>, CategoryRepo>();
            services.AddTransient<IJwtSecurity, JwtSecurity>();
            services.Configure<JwSettings>(configuration.GetSection("JwSettings"));
            
            services.AddDbContext<SimpleadminContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: dbName);
            });

            mockActionContext.HttpContext.RequestServices = services.BuildServiceProvider();

            var authorizationFilterContext = new Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext(mockActionContext, new List<IFilterMetadata>());

            OkObjectResult tokenresult = (OkObjectResult)new LoginController(new UserRepo(dbFixture.DBOption),
                new JwtSecurity(jwOption)).GetToken(new LoginModel()
                {
                    password = "123456",
                    email = "Eamil@domian.com"
                });
            LoginResultModel loginResult = (LoginResultModel)tokenresult.Value;
            authorizationFilterContext.HttpContext.Request.Headers["Authorization"] = "Bearer " + loginResult.Token;
            attr.OnAuthorization(authorizationFilterContext);
        }
    }
}
