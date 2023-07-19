using Microsoft.EntityFrameworkCore;
using NLog.Web;
using ServiceStationAPI;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Middleware;
using ServiceStationAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ServiceStationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ServiceStationConnection")));
builder.Services.AddControllers();
builder.Services.AddScoped<Seeder>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
seeder.Seed();
// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();