namespace CampusMarketplace.Api.Models;

/// <summary>
/// Represents feedback left after a transaction between users.
/// </summary>
public class Review
{
    public int Id { get; set; }
    public string ReviewerId { get; set; } = default!;
    public AppUser Reviewer { get; set; } = default!;
    public string RevieweeId { get; set; } = default!;
    public AppUser Reviewee { get; set; } = default!;
    public int OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public int Rating { get; set; } // 1..5
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
