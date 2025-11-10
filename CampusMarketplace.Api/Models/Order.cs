namespace CampusMarketplace.Api.Models;

/// <summary>
/// Represents a confirmed purchase between buyer and seller.
/// </summary>
public class Order
{
    public int Id { get; set; }                             // Unique ID for the order
    public string BuyerId { get; set; } = default!;         // Foreign key to the buyer (AppUser)
    public AppUser Buyer { get; set; } = default!;          // Navigation property for the buyer
    public int ListingId { get; set; }                      // Foreign key to the purchased listing
    public Listing Listing { get; set; } = default!;        // Navigation property for the listing
    public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Date when the order was created
    public string Status { get; set; } = "Pending";         // Order status: Pending, Completed, or Cancelled
    public Review? Review { get; set; }                     // Optional review left by the buyer
}
