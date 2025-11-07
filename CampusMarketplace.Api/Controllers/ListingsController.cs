using CampusMarketplace.Api.Models;
using CampusMarketplace.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusMarketplace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    public ListingsController(IUnitOfWork uow) => _uow = uow;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Listing>>> Get() =>
        Ok(await _uow.Listings.GetAllAsync());

    [Authorize(Roles = "Seller,Admin")]
    [HttpPost]
    public async Task<ActionResult> Create(Listing dto)
    {
        await _uow.Listings.AddAsync(dto);
        await _uow.SaveAsync();
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }
}
