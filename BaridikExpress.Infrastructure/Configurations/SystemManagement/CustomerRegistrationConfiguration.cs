using BaridikExpress.Domain.Entities.SystemManagment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.SystemManagement;

public class CustomerRegistrationConfiguration : IEntityTypeConfiguration<CustomerRegistration>
{
    public void Configure(EntityTypeBuilder<CustomerRegistration> builder)
    {
        builder.ToTable("CustomerRegistrationTerms");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.DescriptionAr).HasColumnType("nvarchar(max)").IsRequired(false);
        builder.Property(x => x.DescriptionEn).HasColumnType("nvarchar(max)").IsRequired(false);
    }
}