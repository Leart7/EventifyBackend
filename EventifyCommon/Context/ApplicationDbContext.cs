using EventifyCommon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EventifyCommon.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var userRoleId = "d6df3320-8552-4765-91de-800a887adbea";
            var adminRoleId = "0f79654d-c70d-436e-8fba-9ac2b6ba5413";
            var superAdminRoleId = "f5d237e3-7765-40db-8e2c-acae9226bd89";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "User",
                    NormalizedName = "User".ToUpper(),
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                },
                 new IdentityRole
                {
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId,
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin".ToUpper(),
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            builder.Entity<Event>()
               .HasOne(e => e.User)
               .WithMany(u => u.Events)
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Follow>()
              .HasOne(f => f.Follower)
              .WithMany(u => u.FollowerFollows)
              .HasForeignKey(f => f.FollowerId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Follow>()
                .HasOne(f => f.FollowedUser)
                .WithMany(u => u.FollowedUserFollows)
                .HasForeignKey(f => f.FollowedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Follow>()
                .HasIndex(f => new { f.FollowerId, f.FollowedUserId })
                .IsUnique();

            builder.Entity<Like>()
                .HasIndex(l => new { l.UserId, l.EventId})
                .IsUnique();



            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }
        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<EventAgend> EventAgends { get; set; }
        public DbSet<Format> Formats { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<ClosedAccount> ClosedAccounts { get; set; }
        public DbSet<ClosedAccountReason> ClosedAccountReasons { get; set; }
        public DbSet<ReportEvent> ReportEvents { get; set; }
        public DbSet<ReportEventReason> ReportEventReasons { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Models.Type> Types { get; set; }
        public DbSet<Permission> Permissions { get; set; }


    }

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName).HasMaxLength(255);
            builder.Property(u => u.LastName).HasMaxLength(255);
        }
    }
}
