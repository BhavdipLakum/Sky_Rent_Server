using Sky_Rent.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Sky_Rent.Domain.Interfaces;
using Sky_Rent.Infrastructure.Repositories;
using Sky_Rent.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Enable Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"])),
            ClockSkew = TimeSpan.Zero
        };
    });

// Register custom services and repositories
builder.Services.AddScoped<AuthenticationService>();  // <-- Add this line for authentication service
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Register UserRepository if not already done
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()  // Allow requests from any origin (Netlify frontend can make requests)
            .AllowAnyMethod()  // Allow any HTTP method (GET, POST, etc.)
            .AllowAnyHeader());  // Allow any header
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

// Enable CORS globally
app.UseCors("AllowAll");

app.UseHttpsRedirection();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers to routes
app.MapControllers();

// Run the app
app.Run();
