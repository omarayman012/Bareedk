using BaridikExpress.Domain.Entities.SystemManagment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.SystemManagement;

public class TermsAndConditionsConfiguration : IEntityTypeConfiguration<TermsAndConditions>
{
    public void Configure(EntityTypeBuilder<TermsAndConditions> builder)
    {
        builder.ToTable("TermsAndConditions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.DescriptionAr).HasColumnType("nvarchar(max)").IsRequired(false);
        builder.Property(x => x.DescriptionEn).HasColumnType("nvarchar(max)").IsRequired(false);
    }
}