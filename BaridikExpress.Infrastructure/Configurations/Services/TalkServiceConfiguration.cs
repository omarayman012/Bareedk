using BaridikExpress.Domain.Entities.Services;
using BaridikExpress.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace BaridikExpress.Infrastructure.Configurations.Services;

public class TalkServiceConfiguration : IEntityTypeConfiguration<TalkService>
{
    public void Configure(EntityTypeBuilder<TalkService> builder)
    {
        builder.ToTable("TalkServices");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        #region Personal Info

        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.WorkEmail)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.PostalCode)
            .HasMaxLength(20)
            .IsRequired();

        #endregion

        #region Company Info

        builder.Property(x => x.JobTitle)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.CompanyName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.CompanyAddress)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.WebsiteUrl)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.AdditionalInformation)
            .HasMaxLength(2000)
            .IsRequired();

        #endregion

        #region Enums & Status

        builder.Property(x => x.ShipmentVolumeRange)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue("Pending");

        #endregion

        #region Location

        builder.Property(x => x.VillageId)
            .IsRequired(false);

        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Government)
            .WithMany()
            .HasForeignKey(x => x.GovernmentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Village)
            .WithMany()
            .HasForeignKey(x => x.VillageId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion

        #region Relations

        builder.HasOne(x => x.ServiceBusinessPlan)
            .WithMany()
            .HasForeignKey(x => x.ServiceBusinessPlanId)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion

        #region Indexes

        builder.HasIndex(x => x.WorkEmail);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.ServiceBusinessPlanId);

        #endregion
    }
}