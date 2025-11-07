namespace CampusMarketplace.Api.Models;

/// <summary>
/// Represents a book category (e.g., Science, Literature, Mathematics).
/// </summary>
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
