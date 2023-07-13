using Microsoft.EntityFrameworkCore;
using ServiceStationAPI;
using ServiceStationAPI.Entities;
using ServiceStationAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ServiceStationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ServiceStationConnection")));
builder.Services.AddScoped<Seeder>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
seeder.Seed();
// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Run();