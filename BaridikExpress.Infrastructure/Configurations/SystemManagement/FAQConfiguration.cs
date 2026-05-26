using BaridikExpress.Domain.Entities.SystemManagment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaridikExpress.Infrastructure.Configurations.SystemManagement;

public class FAQConfiguration : IEntityTypeConfiguration<FAQ>
{
    public void Configure(EntityTypeBuilder<FAQ> builder)
    {
        builder.ToTable("FAQs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.QuestionAr).HasMaxLength(500).IsRequired();
        builder.Property(x => x.QuestionEn).HasMaxLength(500).IsRequired();
        builder.Property(x => x.AnswerAr).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(x => x.AnswerEn).HasColumnType("nvarchar(max)").IsRequired();
    }
}