using BE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BE.Infrastructure.Persistence.Configurations
{
    public class ApprovalConfiguration : IEntityTypeConfiguration<Approval>
    {
        public void Configure(EntityTypeBuilder<Approval> builder)
        {
            builder.ToTable("Approvals");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ActionType).HasConversion<string>().HasMaxLength(50);

            builder.HasOne<Step>()
                .WithMany()
                .HasForeignKey(x => x.StepId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Group>()
                .WithMany()
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
