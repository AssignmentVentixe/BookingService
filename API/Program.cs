using API.Data.Contexts;
using API.Data.Repositories;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//Database
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddOpenApi();
var app = builder.Build();

app.MapOpenApi();

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthorization();

app.MapControllers();

app.Run();