using BaridikExpress.Domain.Entities.AuthModules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.UserConfiguration
{
    internal class SubAdminEmployeeConfiguration : IEntityTypeConfiguration<SubAdminEmployee>
    {
        public void Configure(EntityTypeBuilder<SubAdminEmployee> builder)
        {
            builder.ToTable("SubAdminEmployees");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.Gender)
                   .HasMaxLength(20)
                   .IsRequired(false);

            builder.Property(x => x.RoleId)
                  .HasMaxLength(450)
                 .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .IsRequired();

            builder.Property(x => x.CreatedById)
                   .IsRequired(false);

            builder.Property(x => x.UpdatedAt)
                   .IsRequired(false);

            builder.Property(x => x.UpdatedById)
                   .IsRequired(false);

            builder.HasOne(x => x.User)
                   .WithOne(x => x.SubAdminEmployee)
                   .HasForeignKey<SubAdminEmployee>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Role)
                   .WithMany()
                   .HasForeignKey(x => x.RoleId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}