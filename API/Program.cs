using System.Net.Http.Headers;
using System.Text;
using API.Data.Contexts;
using API.Data.Repositories;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddHttpClient("EventService", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["EventService:BaseUrl"]!);
    client.DefaultRequestHeaders.Accept
          .Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient("EmailVerificationProvider", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["EmailVerificationProvider:BaseUrl"]!);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["JwtKey"]!))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwt"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddCors(o =>
    o.AddPolicy("CorsPolicy", p =>
        p.WithOrigins("https://lively-hill-0b76ba003.6.azurestaticapps.net")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()));

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
