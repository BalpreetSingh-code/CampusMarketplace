using CampusMarketplace.Api.Data;
using CampusMarketplace.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CampusMarketplace.Api.Seed;

//
// DbSeeder.cs — sets up default roles, users, and sample data
//
public static class DbSeeder
{
    // Main method that runs all the seeding steps
    public static async Task SeedAsync(IServiceProvider sp)
    {
        // Create a scoped service provider to access the database and identity services
        using var scope = sp.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>(); // Database context
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>(); // Manages user accounts
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(); // Manages roles

        await ctx.Database.MigrateAsync(); // Apply any pending migrations before seeding data

        //
        // --- Seed Roles ---
        // Create basic roles (Admin, Seller, Buyer) if they don't already exist
        //
        string[] roles = { "Admin", "Seller", "Buyer" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))           // Check if role exists
                await roleManager.CreateAsync(new IdentityRole(role)); // Create it if missing
        }

        //
        // --- Seed Users ---
        // Helper method to create users and assign them to a role if they don’t exist
        //
        async Task<AppUser> EnsureUser(string email, string role)
        {
            var user = await userManager.FindByEmailAsync(email); // Look for existing user
            if (user == null)
            {
                // Create new user with default password and confirmed email
                user = new AppUser { UserName = email, Email = email, EmailConfirmed = true };
                await userManager.CreateAsync(user, "Passw0rd!"); // Default password
                await userManager.AddToRoleAsync(user, role);     // Assign user to role
            }
            return user;
        }

        // Create one user for each role
        var admin = await EnsureUser("admin@campus.local", "Admin");
        var seller = await EnsureUser("alice@campus.local", "Seller");
        var buyer = await EnsureUser("bob@campus.local", "Buyer");

        //
        // --- Seed Categories ---
        // Add some default book categories if there are none in the database
        //
        if (!ctx.Categories.Any())
        {
            ctx.Categories.AddRange(
                new Category { Name = "Computer Science", Description = "Programming and tech books" },
                new Category { Name = "Mathematics", Description = "Algebra, calculus, and statistics" },
                new Category { Name = "Literature", Description = "Novels and plays" }
            );
            await ctx.SaveChangesAsync(); // Save categories to database
        }

        //
        // --- Seed Listings ---
        // Add one sample book listing if the Listings table is empty
        //
        if (!ctx.Listings.Any())
        {
            var cs = ctx.Categories.First(c => c.Name == "Computer Science"); // Get category by name
            ctx.Listings.Add(new Listing
            {
                Title = "Discrete Mathematics (3rd Edition)",
                Description = "Clean copy with minimal notes.",
                Price = 40m,                        // Price in dollars
                Condition = "Very Good",            // Book condition
                CategoryId = cs.Id,                 // Link to Computer Science category
                SellerId = seller.Id                // Assign listing to the seller user
            });
            await ctx.SaveChangesAsync();           // Save listing to database
        }

        // Log success message to console/output
        Log.Information("Database seeding completed successfully.");
    }
}
