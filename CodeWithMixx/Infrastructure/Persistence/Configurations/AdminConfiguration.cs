using CodeWithMixx.Domain.Entities.Admins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMixx.Infrastructure.Persistence.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasKey(x => x.AppUserId);
        
        builder.HasOne(x => x.AppUser)
            .WithOne(x => x.Admin)
            .HasForeignKey<Admin>(x => x.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.Reservations)
            .WithOne(x => x.Admin)
            .HasForeignKey(x => x.AdminId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}