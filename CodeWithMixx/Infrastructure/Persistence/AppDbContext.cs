using CodeWithMixx.Domain.Entities.Admins;
using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Domain.Entities.Projects;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Domain.Entities.Students;
using CodeWithMixx.Domain.Entities.Subjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>().ToTable("AppUsers");
        modelBuilder.Entity<IdentityRole>().ToTable("AppRoles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AppUserTokens");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}