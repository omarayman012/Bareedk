using BaridikExpress.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.BlogsModules.DTOs;


public class BlogDetailsResponse
{
    public Guid Id { get; set; }

    public NameDto Title { get; set; }
    public DescriptionDto Description { get; set; }

    public string Image { get; set; }

    public LookupDto Category { get; set; }
    public LookupDto Author { get; set; }
    public LookupDto PublishingHouse { get; set; }

    public TagsDto Tags { get; set; } = new();
    public bool IsActive { get; set; }
    public int Likes { get; set; }          
    public int Dislikes { get; set; }       
    public ReactionType? MyReaction { get; set; }
    public BlogSeoResponse? Seo { get; set; }
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }


    public DateOnly? CreatedDate { get; set; }
    public TimeOnly? CreatedTime { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string? UpdatedBy { get; set; } = string.Empty;
    public PaginatedList<CommentResponse> Comments { get; set; }
}
public class TagsDto
{
    public List<string> Ar { get; set; } = new();
    public List<string> En { get; set; } = new();
}
