namespace CampusMarketplace.Api.Models;

/// <summary>
/// Represents a buyer’s offer on a listing before an order is confirmed.
/// </summary>
public class Offer
{
    public int Id { get; set; }
    public string BuyerId { get; set; } = default!;
    public AppUser Buyer { get; set; } = default!;
    public int ListingId { get; set; }
    public Listing Listing { get; set; } = default!;
    public decimal OfferedPrice { get; set; }
    public string Status { get; set; } = "Pending"; // Pending/Accepted/Rejected
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
