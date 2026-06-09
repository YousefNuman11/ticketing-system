using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TicketingSystem.Repository.Data;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Service;
using TicketingSystem.Services.Service.Abstraction;
using TicketingSystem.Services.Settings;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

//DbContext
builder.Services.AddDbContext<TicketingSystemDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

//UnitOfWork and Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IProductService, ProductService>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

//Settings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

//JWT

var jwtSettings =
    builder.Configuration.GetSection("JwtSettings")
                         .Get<JwtSettings>();


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
    });



var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TicketingSystemDbContext>();

    if (!context.Users.Any())
    {
        var hasher = new PasswordHasher<User>();

        var manager = new User
        {
            FullName = "Yusef Alnuman",
            Email = "yusef@company.com",
            MobileNumber = "0790000000",
            DateOfBirth = new DateTime(2003, 9, 11),
            Role = UserRole.Manager,
            Address = "Amman",
            IsActive = true
        };

        manager.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Yusef123");

        context.Users.Add(manager);
        context.SaveChanges();
    }
}


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
