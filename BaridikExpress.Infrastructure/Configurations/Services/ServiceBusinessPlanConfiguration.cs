using BaridikExpress.Domain.Entities.ServiceModules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Services
{
    public class ServiceBusinessPlanConfiguration : IEntityTypeConfiguration<ServiceBusinessPlan>
    {
        public void Configure(EntityTypeBuilder<ServiceBusinessPlan> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.DescriptionEn)
                .HasMaxLength(2000);

            builder.Property(x => x.DescriptionAr)
                .HasMaxLength(2000);

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            builder.HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UpdatedBy)
                .WithMany()
                .HasForeignKey(x => x.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}