using CodeWithMixx.Domain.Entities.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMixx.Infrastructure.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable(t => t.HasCheckConstraint("CK_Reservations_TotalPrice_Positive", "\"TotalPrice\" >= 0"));
        
        builder.Property(r => r.TotalPrice)
            .HasPrecision(18, 2);

        builder.Property(r => r.PaymentStatus)
            .HasConversion<string>();

        builder.Property(r => r.ServiceType)
            .HasConversion<string>();
        
        builder.HasOne(r => r.Admin)
            .WithMany(a => a.Reservations)
            .HasForeignKey(r => r.AdminId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(r => r.Student)
            .WithMany(s => s.Reservations)
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}