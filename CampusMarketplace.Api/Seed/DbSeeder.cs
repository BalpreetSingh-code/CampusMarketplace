using CampusMarketplace.Api.Data;
using CampusMarketplace.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CampusMarketplace.Api.Seed;

/// <summary>
/// Seeds initial roles, users, and sample data for Campus Marketplace.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider sp)
    {
        using var scope = sp.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await ctx.Database.MigrateAsync();

        // --- Seed Roles ---
        string[] roles = { "Admin", "Seller", "Buyer" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // --- Seed Users ---
        async Task<AppUser> EnsureUser(string email, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new AppUser { UserName = email, Email = email, EmailConfirmed = true };
                await userManager.CreateAsync(user, "Passw0rd!");
                await userManager.AddToRoleAsync(user, role);
            }
            return user;
        }

        var admin = await EnsureUser("admin@campus.local", "Admin");
        var seller = await EnsureUser("alice@campus.local", "Seller");
        var buyer = await EnsureUser("bob@campus.local", "Buyer");

        // --- Seed Categories ---
        if (!ctx.Categories.Any())
        {
            ctx.Categories.AddRange(
                new Category { Name = "Computer Science", Description = "Programming and tech books" },
                new Category { Name = "Mathematics", Description = "Algebra, calculus, and statistics" },
                new Category { Name = "Literature", Description = "Novels and plays" }
            );
            await ctx.SaveChangesAsync();
        }

        // --- Seed Listings ---
        if (!ctx.Listings.Any())
        {
            var cs = ctx.Categories.First(c => c.Name == "Computer Science");
            ctx.Listings.Add(new Listing
            {
                Title = "Discrete Mathematics (3rd Edition)",
                Description = "Clean copy with minimal notes.",
                Price = 40m,
                Condition = "Very Good",
                CategoryId = cs.Id,
                SellerId = seller.Id
            });
            await ctx.SaveChangesAsync();
        }

        Log.Information("✅ Database seeding completed successfully.");
    }
}
