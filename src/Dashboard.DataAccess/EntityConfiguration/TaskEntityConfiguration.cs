using Dashboard.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dashboard.DataAccess.EntityConfiguration;

public class TaskEntityConfiguration : IEntityTypeConfiguration<TaskModel>
{
    public void Configure(EntityTypeBuilder<TaskModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.DueDate).IsRequired(true);
        builder.Property(x => x.Priority).IsRequired(true);
        builder.Property(x => x.Status).IsRequired(true);

        builder.ToTable("Tasks");
    }
}