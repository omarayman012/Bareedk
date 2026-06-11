namespace BaridikExpress.Application.Features.BlogsModules.DTOs;

public class BlogListResponse
{
    public Guid Id { get; set; }
    public NameDto Title { get; set; }
    public DescriptionDto Description { get; set; }
    public string Image { get; set; }
    public LookupDto Category { get; set; }
    public LookupDto Author { get; set; }
    public LookupDto PublishingHouse { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public TagsDto Tags { get; set; } = new();
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public DateOnly? CreatedDate { get; set; }
    public TimeOnly? CreatedTime { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string? UpdatedBy { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class NameDto
{
    public string? Ar { get; set; }
    public string? En { get; set; }
}
public class LookupDto
{
    public Guid Id { get; set; }
    public NameDto Name { get; set; }
}
public class DescriptionDto
{
    public string? Ar { get; set; }
    public string? En { get; set; }
}
