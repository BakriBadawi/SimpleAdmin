using Microsoft.EntityFrameworkCore;
using SimpleAdmin.API.JwHelper;

var builder = WebApplication.CreateBuilder(args);
string conn = builder.Configuration.GetConnectionString("simpleAdmin");
// Add services to the container.
builder.Services.AddDbContext<SimpleadminContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("simpleAdmin"));
});

builder.Services.Configure<JwSettings>(builder.Configuration.GetSection("JwSettings"));
builder.Services.AddTransient<IRepo<User, int>, UserRepo>();
builder.Services.AddTransient<IRepo<Item, Guid>, ItemRepo>();
builder.Services.AddTransient<IRepo<Category, int>, CategoryRepo>();
builder.Services.AddTransient<IJwtSecurity, JwtSecurity>();

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
