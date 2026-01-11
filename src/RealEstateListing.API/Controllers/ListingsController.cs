using Microsoft.AspNetCore.Mvc;
using RealEstateListing.API.ViewModels;
using RealEstateListing.Application.Services;

namespace RealEstateListing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController(IListingService service) : ControllerBase
{
    /// <summary>
    /// Get all listings
    /// </summary>
    [HttpGet]
    [Tags("Listings Retrieval")]
    [ProducesResponseType(typeof(IEnumerable<ListingResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListingResponse>>> GetAll(CancellationToken ct)
    {
        var dtos = await service.GetAllAsync(ct);
        return Ok(dtos.ToResponseList());
    }

    /// <summary>
    /// Get listing by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [Tags("Listings Retrieval")]
    [ProducesResponseType(typeof(ListingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ListingResponse>> GetById(Guid id, CancellationToken ct)
    {
        var dto = await service.GetByIdAsync(id, ct);
        if (dto is null) return NotFound();

        return Ok(dto.ToResponse());
    }

    /// <summary>
    /// Create a new listing (status: Draft)
    /// </summary>
    [HttpPost]
    [Tags("Listings Management")]
    [ProducesResponseType(typeof(ListingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ListingResponse>> Create([FromBody] CreateListingRequest request, CancellationToken ct)
    {
        var createDto = request.ToDto();
        var resultDto = await service.CreateAsync(createDto, ct);
        var response = resultDto.ToResponse();

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Update listing details
    /// </summary>
    [HttpPut("{id:guid}")]
    [Tags("Listings Management")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateListingRequest request, CancellationToken ct)
    {
        var updateDto = request.ToDto();
        await service.UpdateAsync(id, updateDto, ct);
        return NoContent();
    }

    /// <summary>
    /// Publish a draft listing
    /// </summary>
    [HttpPatch("{id:guid}/publish")]
    [Tags("Listings Management")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Publish(Guid id, CancellationToken ct)
    {
        await service.PublishAsync(id, ct);
        return NoContent();
    }

    /// <summary>
    /// Archive a published listing
    /// </summary>
    [HttpPatch("{id:guid}/archive")]
    [Tags("Listings Management")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Archive(Guid id, CancellationToken ct)
    {
        await service.ArchiveAsync(id, ct);
        return NoContent();
    }

    /// <summary>
    /// Delete a listing
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Tags("Listings Management")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}
