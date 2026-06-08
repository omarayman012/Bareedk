namespace BaridikExpress.Application.Features.BlogsModules.DTOs;
public class CommentResponse
{
    public Guid Id { get; set; }

    public string FullName { get; set; }
    public string Photo { get; set; }

    public string Content { get; set; }

    public int LikesCount { get; set; }
    public int DislikesCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public int RepliesCount { get; set; }

    public List<CommentResponse> Replies { get; set; } = new();
}
