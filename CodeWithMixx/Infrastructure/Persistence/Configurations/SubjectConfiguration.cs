using CodeWithMixx.Domain.Entities.Subjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMixx.Infrastructure.Persistence.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasOne(s => s.Admin)
            .WithMany(a => a.Subjects)
            .HasForeignKey(s => s.AdminId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(s => s.Classes)
            .WithOne(c => c.Subject)
            .HasForeignKey(c => c.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(s => s.Projects)
            .WithOne(c => c.Subject)
            .HasForeignKey(c => c.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);
            
        
        builder.HasData(
            new Subject { Id = 1, Name = "Programerski alati"},
            new Subject { Id = 2, Name = "Praktikum primenjenog programiranja"},
            new Subject { Id = 3, Name = "Web programiranje"},
            new Subject { Id = 4, Name = "Osnove C programiranja"},
            new Subject { Id = 5, Name = "Objektno orijentisano programiranje"},
            new Subject { Id = 6, Name = "Internet programerski alati"},
            new Subject { Id = 7, Name = "Baza podataka"}
        );
        
    }
}