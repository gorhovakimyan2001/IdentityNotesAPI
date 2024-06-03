using IdentityDb.Data;
using IdentityDb.Repositories;
using IdentityDb.UnitOfWork;
using IdentityServiceProject.IService;
using IdentityServiceProject.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseLazyLoadingProxies();
});
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string tonkenKeyString = builder.Configuration.GetSection("AppSettings").GetSection("TokenKey").Value ?? string.Empty;

SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
    string.IsNullOrEmpty(tonkenKeyString) ? string.Empty : tonkenKeyString)
    );

TokenValidationParameters tokenValidation = new TokenValidationParameters()
{
    IssuerSigningKey = tokenKey,
    ValidateIssuer = false,
    ValidateIssuerSigningKey = false,
    ValidateAudience = false,
    ClockSkew = TimeSpan.Zero,
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidation;
})
.AddCookie(IdentityConstants.ApplicationScheme);

builder.Services.AddAuthorization();

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddApiEndpoints();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();

app.MapControllers();

app.Run();
