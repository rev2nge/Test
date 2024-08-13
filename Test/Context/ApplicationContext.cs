using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Announcement> Announcements { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>().HasData(
            //    new User
            //    {
            //        Id = Guid.Parse("F30CB1CA-93DC-4321-AE12-06CD3082814A"),
            //        Name = "User2",
            //        Admin = true
            //    },
            //    new User
            //    {
            //        Id = Guid.Parse("47CD18CD-0E5F-4F52-B45F-CB8F98E93588"),
            //        Name = "User1",
            //        Admin = false
            //    }
            //);
        }
    }
}