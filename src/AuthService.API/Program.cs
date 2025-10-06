using System.Security.Cryptography;
using AuthService.DAL.Context;
using AuthService.Services.Helpers.Options;
using AuthService.Services.Services.Tokens;
using AuthService.Services.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var jwtConfig = builder.Configuration.GetSection("JwtConfig");

var connectionString = builder.Configuration.GetConnectionString("CredentialsDatabase");
builder.Services.AddDbContext<CredentialsDatabaseContext>(options => options.UseSqlServer(connectionString));

builder.Services.Configure<JwtOptions>(jwtConfig);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    var rsa = RSA.Create();
    rsa.ImportFromPem(File.ReadAllText(jwtConfig["PublicKeyPath"]));

    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"],
        IssuerSigningKey = new RsaSecurityKey(rsa),
        ClockSkew = TimeSpan.FromMinutes(10)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOrCanViewUsers", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || 
            context.User.HasClaim("permission", "ViewUsers")
        ));
});


builder.Services.AddScoped<ITokenService, TokenService>();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// TODO: fix duplicate email/username exception crashing the program