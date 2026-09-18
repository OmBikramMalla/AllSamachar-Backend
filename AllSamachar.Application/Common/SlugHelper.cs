using System.Text.RegularExpressions;

namespace AllSamachar.Application.Common;

public static class SlugHelper
{
    public static string GenerateSlug(string title)
    {
        var slug = title.ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-").Trim('-');

        if (slug.Length > 60)
        {
            slug = slug[..60];
            var lastDash = slug.LastIndexOf('-');
            if (lastDash > 0) slug = slug[..lastDash];
        }

        return slug;
    }
}