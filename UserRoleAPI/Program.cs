using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using UserRoleAPI.BusinessLayer.Services;
using UserRoleAPI.Data;
using UserRoleAPI.DataAccessLayer.Repositories;
using UserRoleAPI.Logging;
using UserRoleAPI.Logging.Interfaces;

var builder = WebApplication.CreateBuilder(args);

if (!Directory.Exists("Logs"))
{
    Directory.CreateDirectory("Logs");
}

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File($"Logs/log-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddDbContext<UserRoleDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<TokenService>();
builder.Services.AddAutoMapper(typeof(Program));

// JWT authentication configuration
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<IExportExcelService, ExportExcelService>();
builder.Services.AddScoped<IExportPdfService, ExportPdfService>();
builder.Services.AddScoped<ILogService, LogService>();

// Configure email settings
var emailConfig = builder.Configuration.GetSection("EmailSettings");
string fromEmail = emailConfig["FromEmail"];
string displayName = emailConfig["DisplayName"];
string smtpHost = emailConfig["Host"];
int smtpPort = int.Parse(emailConfig["Port"]);
bool enableSsl = bool.Parse(emailConfig["EnableSsl"]);
string appPassword = emailConfig["AppPassword"];

builder.Services
    .AddFluentEmail(fromEmail, displayName)
    .AddSmtpSender(new SmtpClient(smtpHost, smtpPort)
    {
        EnableSsl = enableSsl,
        Credentials = new NetworkCredential(fromEmail, appPassword)
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserRoleAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

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

Log.Information("Application starting up");
app.Lifetime.ApplicationStopping.Register(OnShutdown);

app.Run();

void OnShutdown()
{
    Log.Information("Application is shutting down");
    Log.CloseAndFlush();
}