using Dashboard.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dashboard.DataAccess.EntityConfiguration;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        // properties
        builder.Property(x => x.Address).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Age).IsRequired();
        builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);

        // entity
        builder.ToTable("Users");
    }
}