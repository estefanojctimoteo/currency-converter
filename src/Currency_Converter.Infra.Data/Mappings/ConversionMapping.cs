using Currency_Converter.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Currency_Converter.Domain.Conversions;

namespace Currency_Converter.Infra.Data.Mappings
{
    public class ConversionMapping : EntityTypeConfiguration<Conversion>
    {
        public override void Map(EntityTypeBuilder<Conversion> builder)
        {
            builder.Property(c => c.CurrencyFrom)
               .HasColumnType("varchar(3)")
               .IsRequired();

            builder.Property(c => c.CurrencyTo)
               .HasColumnType("varchar(3)")
               .IsRequired();

            builder.Property(c => c.DateTimeUtc)
               .HasColumnType("datetimeoffset(7)")
               .IsRequired();

            builder.Property(c => c.AmountFrom)
              .HasColumnType("decimal(18,4)")
              .IsRequired();

            builder.Property(c => c.Fee)
              .HasColumnType("decimal(18,4)")
              .IsRequired();

            builder.Ignore(c => c.AmountTo);

            builder.Ignore(c => c.ValidationResult);

            builder.Ignore(c => c.CascadeMode);

            builder.ToTable("Conversion");

            builder.HasOne(c => c.User)
                .WithMany(u => u.Conversion)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}