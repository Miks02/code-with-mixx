using CodeWithMixx.Domain.Entities.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeWithMixx.Infrastructure.Persistence.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.AppUserId);

        builder.HasOne(x => x.AppUser)
            .WithOne(x => x.Student)
            .HasForeignKey<Student>(x => x.AppUserId);
    }
}