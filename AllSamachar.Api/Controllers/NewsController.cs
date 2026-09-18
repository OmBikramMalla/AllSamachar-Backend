using AllSamachar.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllSamachar.Api.Controllers;

[ApiController]
[Route("api/news")]
public class NewsController : ControllerBase
{
    private readonly INewsRepository _newsRepository;

    public NewsController(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatest([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _newsRepository.GetLatestAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var news = await _newsRepository.GetBySlugAsync(slug);
        if (news == null) return NotFound();
        return Ok(news);
    }
}