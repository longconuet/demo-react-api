using DemoReactAPI.Entities;
using DemoReactAPI.Enums;
using DemoReactAPI.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DemoReactAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = Guid.NewGuid(),
                FullName = "Nguyen Thanh Long",
                Email = "nice231096@gmail.com",
                Phone = "0348523140",
                Username = "admin",
                Password = PasswordHelper.HashPasword("admin123"),
                Role = RoleEnum.ADMIN,
                Status = UserStatusEnum.Active
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
