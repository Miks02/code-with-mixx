using CodeWithMixx.Domain.Entities.Projects;
using CodeWithMixx.Domain.Entities.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMixx.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.HasOne(p => p.Reservation)
            .WithOne(r => r.Project)
            .HasForeignKey<Project>(r => r.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(p => p.Subject)
            .WithMany(s => s.Projects)
            .HasForeignKey(p => p.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.ProjectStatus)
            .HasConversion<string>();
        
        builder.HasIndex(p => p.ProjectStatus);
        builder.HasIndex(p => p.StartDate);
        builder.HasIndex(p => p.EndDate);
    }
}