using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.BlogsModules.DTOs
{
    public sealed class CommentExportDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public int RepliesCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
