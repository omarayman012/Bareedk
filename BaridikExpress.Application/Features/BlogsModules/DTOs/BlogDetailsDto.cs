using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.BlogsModules.DTOs
{
    public class BlogDetailsDto
    {
        public Guid Id { get; set; }

        public NameDto Title { get; set; }

        public DescriptionDto Description { get; set; }

        public string Image { get; set; }

        public LookupDto Category { get; set; }

        public LookupDto Author { get; set; }

        public DateOnly? CreatedDate { get; set; }

        public TimeOnly? CreatedTime { get; set; }

        public int CommentsCount { get; set; }

        public int ReactionsCount { get; set; }

        public List<LookupDto> Tags { get; set; } = [];

        public List<CommentDto> Comments { get; set; } = [];
    }
    public class CommentDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }

        public int RepliesCount { get; set; }

        public List<CommentReplyDto> Replies { get; set; } = [];
    }
    public class CommentReplyDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }
    }
}
