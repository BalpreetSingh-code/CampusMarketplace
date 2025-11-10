using Microsoft.AspNetCore.Identity;

namespace CampusMarketplace.Api.Models;

/// <summary>
/// Represents an authenticated user (Admin, Seller, Buyer) with extra profile info.
/// </summary>
public class AppUser : IdentityUser
{
    // Basic profile information
    public string? FirstName { get; set; }         // User's first name
    public string? LastName { get; set; }          // User's last name
    public string? ProfilePictureUrl { get; set; } // Link to profile picture

    //
    // --- Relationships ---
    // These collections represent related data owned by this user
    //
    public ICollection<Listing> Listings { get; set; } = new List<Listing>(); // All listings posted by the user
    public ICollection<Review> Reviews { get; set; } = new List<Review>();    // All reviews written by the user
}