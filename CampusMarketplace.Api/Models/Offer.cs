namespace CampusMarketplace.Api.Models;

/// <summary>
/// Represents a buyer’s offer on a listing before an order is confirmed.
/// </summary>
public class Offer
{
    public int Id { get; set; }                             // Unique ID for the offer
    public string BuyerId { get; set; } = default!;         // Foreign key to the buyer (AppUser)
    public AppUser Buyer { get; set; } = default!;          // Navigation property for the buyer
    public int ListingId { get; set; }                      // Foreign key to the related listing
    public Listing Listing { get; set; } = default!;        // Navigation property for the listing
    public decimal OfferedPrice { get; set; }               // Price the buyer is offering
    public string Status { get; set; } = "Pending";         // Offer status: Pending, Accepted, or Rejected
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp when the offer was created
}
