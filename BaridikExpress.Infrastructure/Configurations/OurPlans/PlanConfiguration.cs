using BaridikExpress.Domain.Entities.OurPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace BaridikExpress.Infrastructure.Configurations.OurPlans
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(250);
            builder.Property(x => x.DescriptionEn)
                .HasMaxLength(2000);

            builder.Property(x => x.DescriptionAr)
                .HasMaxLength(2000);

            builder.Property(x => x.FeaturesEn)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

            builder.Property(x => x.FeaturesAr)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);



            builder.HasMany(x => x.Customers)
            .WithOne(x => x.Plan)
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict);
        }
        }
        }
