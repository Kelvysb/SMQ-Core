using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SMQCore.DataAccess.Interfaces;
using SMQCore.Shared.Models.Entities;

namespace SMQCore.DataAccess.Contexts
{
    public class SMQContext : DbContext, ISMQContext
    {
        private readonly IConfiguration config;

        public DbSet<Message> Messages { get; set; }

        public DbSet<App> Apps { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<UserPermission> UserPermissions { get; set; }

        public DbSet<User> Users { get; set; }

        public SMQContext(DbContextOptions<SMQContext> options, IConfiguration config) : base(options)
        {
            this.config = config;
        }

        public SMQContext(IConfiguration config) : base()
        {
            this.config = config;
        }

        protected SMQContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(config.GetValue<string>("SMQ_CONNECTIONSTRING"));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .ToTable("Messages")
                .HasKey(c => c.Id);

            modelBuilder.Entity<App>()
                .ToTable("Apps")
                .HasKey(c => c.Id);

            modelBuilder.Entity<App>()
                .HasMany(a => a.Messages)
                .WithOne(m => m.App);

            modelBuilder.Entity<App>()
                .HasMany(a => a.Users)
                .WithOne(u => u.App);

            modelBuilder.Entity<User>()
               .ToTable("Users")
               .HasKey(c => c.Id);

            modelBuilder.Entity<Permission>()
                .ToTable("Permissions")
                .HasKey(p => p.Id);

            modelBuilder.Entity<UserPermission>()
                .ToTable("UserPermissions")
                .HasKey(u => new { u.UserId, u.PermissionId });

            modelBuilder.Entity<UserPermission>()
                .HasOne(u => u.User)
                .WithMany(u => u.Permissions);

            modelBuilder.Entity<UserPermission>()
                .HasOne(u => u.Permission)
                .WithMany(u => u.Users);

            modelBuilder.Entity<App>()
                .HasData(
                new App()
                {
                    Id = 1,
                    Key = new Guid().ToString(),
                    Secret = new Guid().ToString(),
                    Description = "MainApp",
                    IsMain = true
                });

            modelBuilder.Entity<User>()
               .HasData(
               new User()
               {
                   Id = 1,
                   AppId = 1,
                   Login = "admin",
                   PasswordHash = "21232f297a57a5a743894a0e4a801fc3"
               });

            modelBuilder.Entity<Permission>()
               .HasData(
               new Permission() { Id = 1, Value = "SuperUser", Enabled = true },
               new Permission() { Id = 2, Value = "AppAdmin", Enabled = true },
               new Permission() { Id = 3, Value = "User", Enabled = true }
               );

            modelBuilder.Entity<UserPermission>()
               .HasData(
                new UserPermission() { UserId = 1, PermissionId = 1 },
                new UserPermission() { UserId = 1, PermissionId = 2 },
                new UserPermission() { UserId = 1, PermissionId = 3 }
                );

            base.OnModelCreating(modelBuilder);
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}