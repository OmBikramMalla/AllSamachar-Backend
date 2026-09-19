using AllSamachar.Application.Common;
using AllSamachar.Application.Dtos;
using AllSamachar.Application.Interfaces;
using AllSamachar.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
        return Ok(new
        {
            items = result.Items.Select(ToDto),
            result.Page,
            result.PageSize,
            result.TotalCount,
            result.TotalPages
        });
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var news = await _newsRepository.GetBySlugAsync(slug);
        if (news == null) return NotFound();
        return Ok(ToDto(news));
    }

    [HttpGet("category/{categorySlug}")]
    public async Task<IActionResult> GetByCategory(string categorySlug, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _newsRepository.GetByCategoryAsync(categorySlug, page, pageSize);
        return Ok(new
        {
            items = result.Items.Select(ToDto),
            result.Page,
            result.PageSize,
            result.TotalCount,
            result.TotalPages
        });
    }

    [HttpGet("publisher/{publisherSlug}")]
    public async Task<IActionResult> GetByPublisher(string publisherSlug, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _newsRepository.GetByPublisherAsync(publisherSlug, page, pageSize);
        return Ok(new
        {
            items = result.Items.Select(ToDto),
            result.Page,
            result.PageSize,
            result.TotalCount,
            result.TotalPages
        });
    }

    [HttpGet("most-read")]
    public async Task<IActionResult> GetMostRead([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _newsRepository.GetMostReadAsync(page, pageSize);
        return Ok(new
        {
            items = result.Items.Select(ToDto),
            result.Page,
            result.PageSize,
            result.TotalCount,
            result.TotalPages
        });
    }

    [HttpGet("breaking")]
    public async Task<IActionResult> GetBreaking()
    {
        var news = await _newsRepository.GetBreakingAsync();
        if (news == null) return NotFound();
        return Ok(ToDto(news));
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] string? category, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _newsRepository.SearchAsync(q, category, page, pageSize);
        return Ok(new
        {
            items = result.Items.Select(ToDto),
            result.Page,
            result.PageSize,
            result.TotalCount,
            result.TotalPages
        });
    }

    [HttpPost("{slug}/view")]
    public async Task<IActionResult> IncrementView(string slug)
    {
        var news = await _newsRepository.GetBySlugAsync(slug);
        if (news == null) return NotFound();

        news.ViewCount += 1;
        await _newsRepository.UpdateAsync(news);
        return NoContent();
    }

    [HttpPost("{slug}/share")]
    public async Task<IActionResult> IncrementShare(string slug)
    {
        var news = await _newsRepository.GetBySlugAsync(slug);
        if (news == null) return NotFound();

        news.ShareCount += 1;
        await _newsRepository.UpdateAsync(news);
        return NoContent();
    }

    [HttpPost("{slug}/click")]
    public async Task<IActionResult> IncrementClick(string slug)
    {
        var news = await _newsRepository.GetBySlugAsync(slug);
        if (news == null) return NotFound();

        news.OriginalClickCount += 1;
        await _newsRepository.UpdateAsync(news);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UpsertNewsRequest request)
    {
        if (request.IsBreaking)
        {
            await ClearExistingBreakingAsync();
        }

        var news = new News
        {
            Id = Guid.NewGuid(),
            Slug = SlugHelper.GenerateSlug(request.Title),
            Title = request.Title,
            Summary = request.Summary,
            ImageUrl = request.ImageUrl,
            CategoryId = request.CategoryId,
            PublisherId = request.PublisherId,
            Author = request.Author,
            OriginalUrl = request.OriginalUrl,
            PublishedAt = request.PublishedAt ?? DateTime.UtcNow,
            Tags = request.Tags,
            Status = Enum.Parse<NewsStatus>(request.Status),
            IsBreaking = request.IsBreaking
        };

        await _newsRepository.AddAsync(news);

        var created = await _newsRepository.GetByIdAsync(news.Id);
        return Ok(ToDto(created!));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpsertNewsRequest request)
    {
        var news = await _newsRepository.GetByIdAsync(id);
        if (news == null) return NotFound();

        if (request.IsBreaking && !news.IsBreaking)
        {
            await ClearExistingBreakingAsync();
        }

        // Slug intentionally never changes on edit — a published URL should stay stable.
        news.Title = request.Title;
        news.Summary = request.Summary;
        news.ImageUrl = request.ImageUrl;
        news.CategoryId = request.CategoryId;
        news.PublisherId = request.PublisherId;
        news.Author = request.Author;
        news.OriginalUrl = request.OriginalUrl;
        news.Tags = request.Tags;
        news.Status = Enum.Parse<NewsStatus>(request.Status);
        news.IsBreaking = request.IsBreaking;

        await _newsRepository.UpdateAsync(news);

        var updated = await _newsRepository.GetByIdAsync(id);
        return Ok(ToDto(updated!));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var news = await _newsRepository.GetByIdAsync(id);
        if (news == null) return NotFound();

        await _newsRepository.DeleteAsync(id);
        return NoContent();
    }

    private async Task ClearExistingBreakingAsync()
    {
        var currentBreaking = await _newsRepository.GetBreakingAsync();
        if (currentBreaking != null)
        {
            currentBreaking.IsBreaking = false;
            await _newsRepository.UpdateAsync(currentBreaking);
        }
    }

    private static NewsDto ToDto(News n) => new()
    {
        Id = n.Id,
        Slug = n.Slug,
        Title = n.Title,
        Summary = n.Summary,
        ImageUrl = n.ImageUrl,
        Category = new CategoryDto { Id = n.Category.Id, Slug = n.Category.Slug, Name = n.Category.Name, AccentColor = n.Category.AccentColor },
        Publisher = new PublisherDto { Id = n.Publisher.Id, Slug = n.Publisher.Slug, Name = n.Publisher.Name, Website = n.Publisher.Website, Description = n.Publisher.Description },
        Author = n.Author,
        OriginalUrl = n.OriginalUrl,
        PublishedAt = n.PublishedAt,
        Tags = n.Tags,
        Status = n.Status.ToString(),
        IsBreaking = n.IsBreaking,
        ViewCount = n.ViewCount,
        ShareCount = n.ShareCount,
        SaveCount = n.SaveCount,
        OriginalClickCount = n.OriginalClickCount
    };

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/all")]
    public async Task<IActionResult> GetAllForAdmin([FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _newsRepository.GetAllForAdminAsync(status, page, pageSize);
        return Ok(new
        {
            items = result.Items.Select(ToDto),
            result.Page,
            result.PageSize,
            result.TotalCount,
            result.TotalPages
        });
    }
}