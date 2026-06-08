namespace BaridikExpress.Application.Features.BlogsModules.DTOs;
public class CommentReactionsResponse
{
    public List<UserReactionDto> Likes { get; set; } = new();
    public List<UserReactionDto> Dislikes { get; set; } = new();
}
