using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.BlogsModules.DTOs
{
    public class BlogReactionsResponse
    {
        public List<UserReactionDto> Likes { get; set; } = new();
        public List<UserReactionDto> Dislikes { get; set; } = new();
    }

    public class UserReactionDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string? Photo { get; set; }
    }
}
