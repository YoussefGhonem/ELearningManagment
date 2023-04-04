using CoursesManagment.Data.DbModels.SecuritySchema;
using Microsoft.EntityFrameworkCore;

namespace CoursesManagment.Data.DataContext
{
    public static class ModelBuilderExtention
    {
        public static void UserModelBuilder(this ModelBuilder modelBuilder)
        {

            // set application user relations
            modelBuilder.Entity<ApplicationUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            // set application role relations
            modelBuilder.Entity<ApplicationRole>(b =>
            {
                // set application role primary key
                b.HasKey(u => u.Id);
                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            // Each Role can have many associated RoleClaims
            modelBuilder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            // Update Identity Schema
            modelBuilder.Entity<ApplicationUser>().ToTable("Users", "Security");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles", "Security");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRoles", "Security");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogins", "Security");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaims", "Security");
            modelBuilder.Entity<ApplicationUserToken>().ToTable("UserTokens", "Security");
            modelBuilder.Entity<ApplicationRoleClaim>().ToTable("RoleClaims", "Security");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
