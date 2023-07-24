using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using ServiceStationAPI;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Middleware;
using ServiceStationAPI.Models;
using ServiceStationAPI.Models.Validators;
using ServiceStationAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };

});

builder.Services.AddDbContext<ServiceStationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ServiceStationConnection")));
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddScoped<Seeder>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IOrderNoteService, OrderNoteService>();
builder.Services.AddScoped<IPasswordHasher<User>,PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterAccountDto>,RegisterAccountDtoValidator>();
builder.Services.AddScoped<IValidator<CreateOrderNoteDto>,CreateOrderNoteDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateVehicleDto>,UpdateVehicleDtoValidator>();
builder.Services.AddScoped<IValidator<CreateVehicleDto>,CreateVehicleDtoValidator>();
builder.Services.AddScoped<IValidator<LoginAccountDto>,LoginAccountDtoValidator>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();
var app = builder.Build();
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
seeder.Seed();
// Configure the HTTP request pipeline
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Service Station API"));

app.Run();