using AuthService.DAL.Context;
using AuthService.Services.Helpers.Options;
using AuthService.Services.Services.Tokens;
using AuthService.Services.Services.Users;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var jwtConfig = builder.Configuration.GetSection("JwtConfig");

var connectionString = builder.Configuration.GetConnectionString("CredentialsDatabase");
builder.Services.AddDbContext<CredentialsDatabaseContext>(options => options.UseSqlServer(connectionString));

builder.Services.Configure<JwtOptions>(jwtConfig);

builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();