using AllSamachar.Application.Dtos;
using AllSamachar.Application.Interfaces;
using AllSamachar.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllSamachar.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var dtos = new List<object>();

        foreach (var c in categories)
        {
            var count = await _categoryRepository.GetNewsCountAsync(c.Id);
            dtos.Add(new
            {
                c.Id,
                c.Slug,
                c.Name,
                c.AccentColor,
                NewsCount = count
            });
        }

        return Ok(dtos);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var category = await _categoryRepository.GetBySlugAsync(slug);
        if (category == null) return NotFound();
        return Ok(new CategoryDto { Id = category.Id, Slug = category.Slug, Name = category.Name, AccentColor = category.AccentColor });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        category.Id = Guid.NewGuid();
        await _categoryRepository.AddAsync(category);
        return Ok(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Category category)
    {
        category.Id = id;
        await _categoryRepository.UpdateAsync(category);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var count = await _categoryRepository.GetNewsCountAsync(id);
        if (count > 0)
        {
            return BadRequest(new { message = $"Cannot delete — {count} stories use this category" });
        }

        await _categoryRepository.DeleteAsync(id);
        return NoContent();
    }
}