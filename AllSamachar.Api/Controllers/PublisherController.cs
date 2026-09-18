using AllSamachar.Application.Dtos;
using AllSamachar.Application.Interfaces;
using AllSamachar.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AllSamachar.Api.Controllers;

[ApiController]
[Route("api/publishers")]
public class PublisherController : ControllerBase
{
    private readonly IPublisherRepository _publisherRepository;

    public PublisherController(IPublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var publishers = await _publisherRepository.GetAllAsync();
        var dtos = new List<object>();

        foreach (var p in publishers)
        {
            var count = await _publisherRepository.GetNewsCountAsync(p.Id);
            dtos.Add(new
            {
                p.Id,
                p.Slug,
                p.Name,
                p.Website,
                p.Description,
                NewsCount = count
            });
        }

        return Ok(dtos);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var publisher = await _publisherRepository.GetBySlugAsync(slug);
        if (publisher == null) return NotFound();
        return Ok(new PublisherDto { Id = publisher.Id, Slug = publisher.Slug, Name = publisher.Name, Website = publisher.Website, Description = publisher.Description });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Publisher publisher)
    {
        publisher.Id = Guid.NewGuid();
        await _publisherRepository.AddAsync(publisher);
        return Ok(publisher);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Publisher publisher)
    {
        publisher.Id = id;
        await _publisherRepository.UpdateAsync(publisher);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var count = await _publisherRepository.GetNewsCountAsync(id);
        if (count > 0)
        {
            return BadRequest(new { message = $"Cannot delete — {count} stories use this source" });
        }

        await _publisherRepository.DeleteAsync(id);
        return NoContent();
    }
}