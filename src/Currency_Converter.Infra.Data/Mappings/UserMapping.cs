using Currency_Converter.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Currency_Converter.Domain.Users;

namespace Currency_Converter.Infra.Data.Mappings
{
    public class UserMapping : EntityTypeConfiguration<User>
    {
        public override void Map(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Email)
               .HasColumnType("varchar(200)")
               .IsRequired();

            builder.Ignore(s => s.ValidationResult);

            builder.Ignore(s => s.CascadeMode);

            builder.ToTable("User");
        }
    }
}