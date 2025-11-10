using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CampusMarketplace.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace CampusMarketplace.Api.Controllers;

//
// AuthController.cs — handles user authentication and JWT token generation
//
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager; // Manages user accounts
    private readonly IConfiguration _config;            // Accesses app configuration settings

    // Constructor: injects UserManager and configuration
    public AuthController(UserManager<AppUser> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    //
    // --- POST: /api/auth/login ---
    // Authenticates a user and returns a JWT token if valid
    //
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState); // If request payload is invalid

        // Look up the user by email
        var user = await _userManager.FindByEmailAsync(request.Email);

        // Verify user existence and password
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            Log.Warning("Failed login attempt for {Email}", request.Email);
            return Unauthorized("Invalid email or password.");
        }

        // Get assigned roles for this user
        var roles = await _userManager.GetRolesAsync(user);

        // Generate JWT token for this user
        var token = GenerateJwtToken(user, roles);

        Log.Information("User {Email} logged in successfully.", request.Email);

        // Return token and user details
        return Ok(new
        {
            token,
            user = new
            {
                user.Email,
                Roles = roles
            }
        });
    }

    //
    // --- Helper Method: GenerateJwtToken ---
    // Creates a signed JWT token containing user claims and roles
    //
    private string GenerateJwtToken(AppUser user, IList<string> roles)
    {
        var jwt = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)); // Secret key from config
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);  // Signing credentials

        // Define claims for the token (user ID, email, roles, etc.)
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),               // Subject (email)
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
            new Claim(ClaimTypes.NameIdentifier, user.Id),                     // User ID
            new Claim(ClaimTypes.Email, user.Email!)                           // User email
        };

        // Add role claims
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        // Create the token
        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],                         // Token issuer
            audience: jwt["Audience"],                     // Token audience
            claims: claims,                                // Claims to include
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwt["ExpiresMinutes"] ?? "60")), // Expiry time
            signingCredentials: creds                      // Signing key
        );

        // Return the token string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

//
// LoginRequest.cs — represents the login request body
//
public class LoginRequest
{
    public string Email { get; set; } = default!;    // User email address
    public string Password { get; set; } = default!; // User password
}
