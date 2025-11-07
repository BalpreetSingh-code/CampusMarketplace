using Microsoft.AspNetCore.Identity;

namespace CampusMarketplace.Api.Models;

/// <summary>
/// Represents an authenticated user (Admin, Seller, Buyer) with extra profile info.
/// </summary>
public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }

    // Navigation properties (optional)
    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}