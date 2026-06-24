using CodeWithMixx.Domain.Admins;
using CodeWithMixx.Domain.AppUsers;
using CodeWithMixx.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMixx.Infrastructure.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("AppUsers");
        
        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(x => x.Admin)
            .WithOne(x => x.AppUser)
            .HasForeignKey<Admin>(x => x.AppUserId);

        builder.HasOne(x => x.Student)
            .WithOne(x => x.AppUser)
            .HasForeignKey<Student>(x => x.AppUserId);
    }
}