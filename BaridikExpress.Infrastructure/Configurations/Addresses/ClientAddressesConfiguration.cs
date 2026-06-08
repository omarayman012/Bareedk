using BaridikExpress.Domain.Entities.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Persistence.Configurations.Addresses;

public class ClientAddressesConfiguration : IEntityTypeConfiguration<ClientAddress>
{
    public void Configure(EntityTypeBuilder<ClientAddress> builder)
    {
        builder.ToTable("ClientAddresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AddressType)
            .IsRequired();

        builder.Property(x => x.Street)
            .HasMaxLength(250);
        builder.Property(x => x.BuildingNumber)
            .HasMaxLength(100);
        builder.Property(x => x.FloorNumber)
            .HasMaxLength(50);

        builder.Property(x => x.FlatNumber)
            .HasMaxLength(100);

        builder.Property(x => x.DistinctiveMark)
            .HasMaxLength(500);

        builder.Property(x => x.ZipCode)
            .HasMaxLength(50);

        builder.Property(x => x.RecipientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.AddressTitle)
            .HasMaxLength(200);

        builder.Property(x => x.Location)
            .HasMaxLength(500);

        builder.Property(x => x.Latitude)
            .HasColumnType("decimal(18,8)");

        builder.Property(x => x.Longitude)
            .HasColumnType("decimal(18,8)");

        builder.Property(x => x.IsDefault)
            .HasDefaultValue(false);


        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        
        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

     
        builder.HasOne(x => x.Government)
            .WithMany()
            .HasForeignKey(x => x.GovernmentId)
            .OnDelete(DeleteBehavior.Restrict);

     
        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);

     
        builder.HasOne(x => x.Village)
            .WithMany()
            .HasForeignKey(x => x.VillageId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
