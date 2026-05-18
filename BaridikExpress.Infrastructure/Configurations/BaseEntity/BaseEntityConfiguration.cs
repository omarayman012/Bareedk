using BaridikExpress.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.BaseEntity
{
    public abstract class BaseEntityConfiguration<TEntity>
        : IEntityTypeConfiguration<TEntity>
        where TEntity : class 
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(nameof(Domain.Entities.Base.BaseEntity.CreatedAt))
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(nameof(Domain.Entities.Base.BaseEntity.UpdatedAt))
                .IsRequired(false);

            builder.Property(nameof(Domain.Entities.Base.BaseEntity.CreatedById))
                .IsRequired(false)
                .HasMaxLength(450);

            builder.Property(nameof(Domain.Entities.Base.BaseEntity.UpdatedById))
                .IsRequired(false)
                .HasMaxLength(450);

            builder.Property(nameof(Domain.Entities.Base.BaseEntity.IsActive))
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}