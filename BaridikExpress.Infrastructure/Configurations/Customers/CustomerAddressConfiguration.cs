using BaridikExpress.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.Customers;

internal sealed class CustomerAddressConfiguration
    : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.ToTable("CustomerAddresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AddressType)
            .IsRequired(false); 

        builder.Property(x => x.Street)
            .HasMaxLength(300)
            .IsRequired(false); 

        builder.Property(x => x.BuildingNumber)
            .HasMaxLength(50)
            .IsRequired(false); 

        builder.Property(x => x.FloorNumber)
            .HasMaxLength(50);

        builder.Property(x => x.DistinctiveMark)
            .HasMaxLength(500);

        builder.Property(x => x.ZipCode)
            .HasMaxLength(20);

        builder.Property(x => x.Location)
            .HasMaxLength(500);

        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryId)
            .IsRequired(false) 
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Government)
            .WithMany()
            .HasForeignKey(x => x.GovernmentId)
            .IsRequired(false) 
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .IsRequired(false) 
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Village)
            .WithMany()
            .HasForeignKey(x => x.VillageId)
            .IsRequired(false) 
            .OnDelete(DeleteBehavior.Restrict);
    }
}