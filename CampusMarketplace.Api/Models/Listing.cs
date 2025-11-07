namespace CampusMarketplace.Api.Models;

/// <summary>
/// Represents a book listing posted by a seller.
/// </summary>
public class Listing
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public string Condition { get; set; } = "Good";

    // Foreign key to Category
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    // Foreign key to Seller
    public string SellerId { get; set; } = default!;
    public AppUser? Seller { get; set; }

    // Navigation collections
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
