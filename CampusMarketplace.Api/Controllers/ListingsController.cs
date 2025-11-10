using CampusMarketplace.Api.Models;
using CampusMarketplace.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusMarketplace.Api.Controllers;

//
// ListingsController.cs — handles all API actions related to listings
//
[ApiController]
[Route("api/[controller]")]
public class ListingsController : ControllerBase
{
    private readonly IUnitOfWork _uow; // Gives access to repositories and save operations

    // Constructor: injects the UnitOfWork for database access
    public ListingsController(IUnitOfWork uow) => _uow = uow;

    //
    // --- GET: /api/listings ---
    // Returns all listings from the database
    //
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Listing>>> Get() =>
        Ok(await _uow.Listings.GetAllAsync());

    //
    // --- POST: /api/listings ---
    // Creates a new listing — only allowed for Sellers or Admins
    //
    [Authorize(Roles = "Seller,Admin")]
    [HttpPost]
    public async Task<ActionResult> Create(Listing dto)
    {
        await _uow.Listings.AddAsync(dto);  // Add new listing
        await _uow.SaveAsync();             // Save changes to the database
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto); // Return 201 with location
    }
}
