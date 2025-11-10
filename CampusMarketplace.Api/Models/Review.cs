namespace CampusMarketplace.Api.Models;

/// <summary>
/// Represents feedback left after a transaction between users.
/// </summary>
public class Review
{
    public int Id { get; set; }                              // Unique ID for the review
    public string ReviewerId { get; set; } = default!;       // Foreign key for the user who wrote the review
    public AppUser Reviewer { get; set; } = default!;        // Navigation property for the reviewer
    public string RevieweeId { get; set; } = default!;       // Foreign key for the user being reviewed
    public AppUser Reviewee { get; set; } = default!;        // Navigation property for the reviewee
    public int OrderId { get; set; }                         // Foreign key to the related order
    public Order Order { get; set; } = default!;             // Navigation property for the order
    public int Rating { get; set; }                          // Rating score (1–5)
    public string? Comment { get; set; }                     // Optional text feedback
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Date the review was created
}