using Microsoft.EntityFrameworkCore;
using WebApi.DAL.Helpers;
using WebApi.DAL.Model;
//using Microsoft.Extensions.Configuration;


namespace WebApi.DAL
{
    public class WebDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> Userroles { get; set; }

        public DbSet<UserToken> Usertokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DefaultConfig.ConnectionString,//@"server=DESKTOP-2E612Q3; database=SampleWebApi; UID=root; PWD=root;",
                    builder =>
                    {
                        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    });
                base.OnConfiguring(optionsBuilder);
            }

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                //entity.Property<int>("UserId").IsRequired().ValueGeneratedOnAdd();
                entity.HasKey("UserId");
                entity.Property(e => e.UserKey).IsRequired();
                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.About);
                entity.Property(e => e.IsArchived).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.DateUpdated).IsRequired();
                entity.Property(e => e.DateCreated).IsRequired();

                //entity.Ignore("UserId");

            });


            modelBuilder.Entity<Role>(entity =>
            {
                //entity.Property<int>("RoleId").IsRequired().ValueGeneratedOnAdd();
                entity.HasKey("RoleId");
                entity.Property(e => e.RoleKey).IsRequired();
                entity.Property(e => e.RoleName).IsRequired();
                entity.Property(e => e.IsArchived).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.DateUpdated).IsRequired();
                entity.Property(e => e.DateCreated).IsRequired();
                //entity.Ignore("RoleId");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                //entity.Property<int>("UserRoleId").IsRequired().ValueGeneratedOnAdd();
                entity.HasKey("UserRoleId");
                entity.Property(e => e.UserRoleKey);
                entity.Property(e => e.UserKey).IsRequired();
                entity.Property(e => e.RoleKey).IsRequired();
                entity.Property(e => e.IsArchived).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.DateUpdated).IsRequired();
                entity.Property(e => e.DateCreated).IsRequired();
                //entity.Ignore("UserRoleId")/*;*/
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                //entity.Property<int>("TokenId").IsRequired().ValueGeneratedOnAdd();
                entity.HasKey("TokenId");
                entity.Property(e => e.UserKey);
                entity.Property(e => e.TokenKey);
                entity.Property(e => e.TokenHash);
                //entity.Ignore("TokenId");
            });
        }
    }
}

