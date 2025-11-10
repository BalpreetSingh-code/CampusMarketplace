using System.Text;
using CampusMarketplace.Api.Data;
using CampusMarketplace.Api.Models;
using CampusMarketplace.Api.Repositories;
using CampusMarketplace.Api.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

//
// Program.cs — main entry point for the Campus Marketplace API
//

var builder = WebApplication.CreateBuilder(args);

//
// --- Logging Setup ---
// Configure Serilog for advanced logging using settings from appsettings.json
//
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

//
// --- Database Configuration ---
// Connect to SQL Server using the connection string defined in appsettings.json
//
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))); // Registers EF Core DbContext

//
// --- Identity Setup ---
// Adds ASP.NET Core Identity with support for roles and token providers
//
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()  // Use EF Core to store identity data
    .AddDefaultTokenProviders();                // Enables token-based actions like password reset

//
// --- JWT Authentication ---
// Configure JSON Web Token authentication for secure API access
//
var jwt = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(opt =>
{
    // Set default authentication and challenge schemes to JWT
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    // Define validation rules for incoming JWT tokens
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"], // The expected issuer (from appsettings)
        ValidAudience = jwt["Audience"], // The expected audience (from appsettings)
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)) // Key used to sign the token
    };
});

builder.Services.AddAuthorization(); // Enables role-based or policy-based authorization

//
// --- Dependency Injection ---
// Register the UnitOfWork and repositories for database operations
//
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();          // Registers controller endpoints
builder.Services.AddEndpointsApiExplorer(); // Enables minimal API endpoint discovery

//
// --- Swagger Setup ---
// Adds Swagger UI for testing the API with JWT authentication support
//
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CampusMarketplace.Api",
        Version = "v1",
        Description = "ASP.NET Core Web API for Campus Marketplace"
    });

    // Add JWT authentication option to Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT token with **Bearer** prefix. Example: `Bearer eyJhbGciOi...`",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Apply the security scheme globally
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

//
// --- Build the App ---
//
var app = builder.Build();

//
// --- Middleware Pipeline ---
// Only enable Swagger in Development mode
//
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(); // Logs all HTTP requests
app.UseHttpsRedirection();      // Redirects HTTP to HTTPS
app.UseAuthentication();        // Enables JWT authentication middleware
app.UseAuthorization();         // Checks user permissions

app.MapControllers();           // Maps controller routes (e.g., /api/products)

//
// --- Database Seeding ---
// Seeds roles, default users, and initial data on startup
//
await DbSeeder.SeedAsync(app.Services);

app.Run(); // Run the web application
