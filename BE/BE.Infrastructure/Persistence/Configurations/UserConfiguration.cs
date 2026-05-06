using BE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BE.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Username).IsRequired().HasMaxLength(100);
            builder.Property(x => x.FullName).HasMaxLength(200);
            builder.Property(x => x.Email).HasMaxLength(150);
            builder.Property(x => x.IsAdmin).HasDefaultValue(false);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
        }
    }
}
