using Microsoft.EntityFrameworkCore;
using ServiceStationAPI.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ServiceStationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ServiceStationConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();