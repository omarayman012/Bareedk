using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Domain.Entities.BlogsModules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Blog
{
    public class BlogConfiguration : IEntityTypeConfiguration<Domain.Entities.BlogsModules.Blog>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.BlogsModules.Blog> builder)
        {
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Blogs)
                .HasForeignKey(x => x.BlogsCategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
    public class CommentReactionConfiguration
    : IEntityTypeConfiguration<CommentReaction>
    {
        public void Configure(EntityTypeBuilder<CommentReaction> builder)
        {
            builder.HasOne(x => x.Comment)
                .WithMany(x => x.Reactions)
                .HasForeignKey(x => x.CommentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

   
}
