using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelatorDataAccess.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TravelatorDataAccess.Interfaces;
using TravelatorDataAccess.Repositories;
using TravelatorService.Interfaces;
using TravelatorService.Services;
using TravelatorService.Mapper;
using TravelatorDataAccess.NotificationHub;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = Environment.GetEnvironmentVariable("Jwt_Key");
var dbConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
var brevoApiKey = Environment.GetEnvironmentVariable("Brevo_ApiKey");

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(dbConnectionString))
{
    throw new Exception("Critical environment variables are missing. Check your configuration.");
}

// Add services to the container
builder.Services.AddSignalR();
builder.Services.AddSingleton<INotificationPublisher, RabbitMQService>();
builder.Services.AddHostedService<RabbitMQService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddScoped<ICabsRepo, CabsRepo>();
builder.Services.AddScoped<ICabsService, CabsService>();
builder.Services.AddScoped<ITripsRepo, TripsRepo>();
builder.Services.AddScoped<ITripsService, TripsService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailService>(sp => new EmailService(brevoApiKey));
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<TravelatorContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "TRavelator",
        ValidAudience = "TRavelator",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["id"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/notificationHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddDbContext<TravelatorContext>(options =>
    options.UseMySql(
        dbConnectionString,
        new MySqlServerVersion(new Version(8, 0, 29))
    )
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("https://travlator.netlify.app", "http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Seed roles and admin user on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    await SeedRolesAndAdminAsync(roleManager, userManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHub<NotificationHub>("/notificationHub");
app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

async Task SeedRolesAndAdminAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    string[] roleNames = { "Admin", "User", "Manager", "Director" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var adminEmail = "admin@travelator.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var newAdmin = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail
        };
        var result = await userManager.CreateAsync(newAdmin, "Admin@1234");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }
}
