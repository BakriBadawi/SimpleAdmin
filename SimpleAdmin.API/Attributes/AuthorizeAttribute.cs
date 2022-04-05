using Microsoft.AspNetCore.Mvc.Filters;
using SimpleAdmin.API.JwHelper;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{

    public static AuthorizeUser AuthUser
    {
        get {return authUser; }
    }


    private static AuthorizeUser authUser;
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var _repo = context.HttpContext.RequestServices.GetService(typeof(IRepo<User, int>)) as IRepo<User, int>;
        var _jwtSecurity = context.HttpContext.RequestServices.GetService(typeof(IJwtSecurity)) as IJwtSecurity;

        authUser = new AuthorizeUser();
        string token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        int? userid= _jwtSecurity.ValidateToken(context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());
        if (userid.HasValue)
        {
            authUser.User = new UserModel(_repo.Get(userid.Value).GetAwaiter().GetResult());
            authUser.IsAutorizes = authUser.User !=null&& authUser.User.DBUser!=null &&authUser.User.isEnabled; 
        }
        if (!authUser.IsAutorizes)
        {
            // not logged in
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}


