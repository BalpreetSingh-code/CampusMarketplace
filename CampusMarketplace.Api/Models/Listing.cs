namespace CampusMarketplace.Api.Models;

/// <summary>
/// Represents a book listing posted by a seller.
/// </summary>
public class Listing
{
    public int Id { get; set; }                       // Unique ID for the listing
    public string Title { get; set; } = default!;     // Title of the listed item
    public string Description { get; set; } = default!; // Short description of the item
    public decimal Price { get; set; }                // Price set by the seller
    public string Condition { get; set; } = "Good";   // Condition (e.g., New, Good, Fair)

    //
    // --- Category Relationship ---
    // Each listing belongs to one category (like Science or Literature)
    //
    public int CategoryId { get; set; }               // Foreign key to Category
    public Category? Category { get; set; }           // Navigation property for Category

    //
    // --- Seller Relationship ---
    // Each listing is posted by a single seller (AppUser)
    //
    public string SellerId { get; set; } = default!;  // Foreign key to Seller (AppUser)
    public AppUser? Seller { get; set; }              // Navigation property for Seller

    //
    // --- Related Collections ---
    // A listing can have multiple offers and orders linked to it
    //
    public ICollection<Offer> Offers { get; set; } = new List<Offer>(); // Offers made by buyers
    public ICollection<Order> Orders { get; set; } = new List<Order>(); // Orders created after purchase
}
