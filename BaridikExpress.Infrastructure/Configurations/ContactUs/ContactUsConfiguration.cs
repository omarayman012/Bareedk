using BaridikExpress.Domain.Entities.ContactUs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.ContactUs;

public class ContactUsConfiguration : IEntityTypeConfiguration<Domain.Entities.ContactUs.ContactUs>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.ContactUs.ContactUs> builder)
    {
        builder.ToTable("ContactUs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Message)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(x => x.IsRead)
            .IsRequired();
    }
}