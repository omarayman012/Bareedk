using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Application.Interfaces;
using BaridikExpress.Application.Interfaces.BlogModules;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.BlogsModules;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaridikExpress.Infrastructure.Services.BlogModules
{
    public class BlogService(IApplicationDbContext context, IFileStorageService fileStorage) : IBlogService
    {
        public async Task<BlogSeo> HandleSeoAsync(CreateBlogSeoDto? seo, Blog blog)
        {
            seo ??= new CreateBlogSeoDto();

            var slugEn = await GenerateUniqueSlugsAsync(
                seo.SlugEn.Count > 0 ? seo.SlugEn : new List<string> { blog.TitleEn },
                blog.Id,
                isEnglish: true);

            var slugAr = await GenerateUniqueSlugsAsync(
                seo.SlugAr.Count > 0 ? seo.SlugAr : new List<string> { blog.TitleAr },
                blog.Id,
                isEnglish: false);

            var metaTitleEn = string.IsNullOrWhiteSpace(seo.MetaTitleEn) ? blog.TitleEn : seo.MetaTitleEn;
            var metaTitleAr = string.IsNullOrWhiteSpace(seo.MetaTitleAr) ? blog.TitleAr : seo.MetaTitleAr;

            var metaDescEn = string.IsNullOrWhiteSpace(seo.MetaDescriptionEn) ? blog.DescriptionEn : seo.MetaDescriptionEn;
            var metaDescAr = string.IsNullOrWhiteSpace(seo.MetaDescriptionAr) ? blog.DescriptionAr : seo.MetaDescriptionAr;

            string? imageUrl = null;
            if (seo.MetaImage is not null)
            {
                await using var stream = seo.MetaImage.OpenReadStream();
                imageUrl = await fileStorage.SaveFileAsync(stream, seo.MetaImage.FileName, "blogs/seo");
            }

            return new BlogSeo
            {
                Id = Guid.NewGuid(),
                BlogId = blog.Id,
                SlugEn = slugEn,
                SlugAr = slugAr,
                MetaTitleEn = TruncateToLength(metaTitleEn, 60),
                MetaTitleAr = TruncateToLength(metaTitleAr, 60),
                MetaDescriptionEn = TruncateToLength(metaDescEn, 160),
                MetaDescriptionAr = TruncateToLength(metaDescAr, 160),
                MetaKeywordsEn = seo.MetaKeywordsEn is not null ? string.Join(",", seo.MetaKeywordsEn) : null,
                MetaKeywordsAr = seo.MetaKeywordsAr is not null ? string.Join(",", seo.MetaKeywordsAr) : null,
                MetaImage = imageUrl ?? blog.Image
            };
        }

        public async Task<List<BlogTag>> HandleTagsAsync(List<TagDto> tags, Guid blogId)
        {
            var normalizedTags = tags
           .Where(x => !string.IsNullOrWhiteSpace(x.NameEn) || !string.IsNullOrWhiteSpace(x.NameAr))
           .Select(x => new
           {
               NameEn = string.IsNullOrWhiteSpace(x.NameEn) ? x.NameAr!.Trim() : x.NameEn.Trim(),
               NameAr = string.IsNullOrWhiteSpace(x.NameAr) ? x.NameEn!.Trim() : x.NameAr.Trim(),
           })
           .Select(x => new
           {
               x.NameEn,
               x.NameAr,
               NormalizedNameEn = x.NameEn.ToLower()
           })
           .DistinctBy(x => x.NormalizedNameEn)
           .ToList();


            var normalizedNames = normalizedTags
                .Select(x => x.NormalizedNameEn)
                .ToList();

            var existingTags = await context.Tags
                .Where(x => normalizedNames.Contains(x.NameEn.ToLower()))
                .ToListAsync();

            var result = new List<BlogTag>();
            var newTags = new List<Tag>();

            foreach (var item in normalizedTags)
            {
                var tag = existingTags.FirstOrDefault(x => x.NameEn.ToLower() == item.NormalizedNameEn);

                if (tag is null)
                {
                    tag = new Tag
                    {
                        Id = Guid.NewGuid(),
                        NameEn = item.NameEn,
                        NameAr = item.NameAr,
                        Slug = GenerateSlug(item.NameEn)
                    };

                    newTags.Add(tag);
                }

                result.Add(new BlogTag
                {
                    BlogId = blogId,
                    Tag = tag
                });
            }

            await context.Tags.AddRangeAsync(newTags);

            return result;
        }

        private async Task<string> GenerateUniqueSlugsAsync(
            List<string> inputs,
            Guid blogId,
            bool isEnglish)
        {
            var slugs = new List<string>();

            foreach (var input in inputs.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var normalizedSlug = GenerateSlug(input);

                var existing = isEnglish
                    ? await context.BlogSeos
                        .Where(x => x.SlugEn != null && x.BlogId != blogId)
                        .Select(x => x.SlugEn)
                        .ToListAsync()
                    : await context.BlogSeos
                        .Where(x => x.SlugAr != null && x.BlogId != blogId)
                        .Select(x => x.SlugAr)
                        .ToListAsync();

                var allExisting = existing
                    .SelectMany(x => x!
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    .ToHashSet();

                if (!allExisting.Contains(normalizedSlug))
                {
                    slugs.Add(normalizedSlug);
                    continue;
                }

                var counter = 1;
                string uniqueSlug;

                do
                {
                    uniqueSlug = $"{normalizedSlug}-{counter++}";
                }
                while (allExisting.Contains(uniqueSlug));

                slugs.Add(uniqueSlug);
            }

            return string.Join(",", slugs.Distinct());
        }

        private static string GenerateSlug(string text)
        {
            text = text.Trim().ToLower();
            text = Regex.Replace(text, @"[^a-z0-9\u0600-\u06FF\s-]", "");
            text = Regex.Replace(text, @"\s+", "-");
            text = Regex.Replace(text, @"-{2,}", "-");
            return text.Trim('-');
        }

        private static string TruncateToLength(string? value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            value = value.Trim();

            return value.Length <= maxLength
                ? value
                : value[..maxLength];
        }
    }
}
