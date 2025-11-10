namespace CampusMarketplace.Api.Models;

/// <summary>
/// Represents a book category (e.g., Science, Literature, Mathematics).
/// </summary>
public class Category
{
    public int Id { get; set; }                     // Unique identifier for the category
    public string Name { get; set; } = default!;    // Name of the category
    public string? Description { get; set; }        // Optional short description

    //
    // --- Relationships ---
    // A category can have many listings under it
    //
    public ICollection<Listing> Listings { get; set; } = new List<Listing>(); // All listings linked to this category
}
