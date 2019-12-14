using Api.LiveHub.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.LiveHub.Infrastrue.DataContext
{
    public class BusinessDbContext:DbContext
    {
        public BusinessDbContext(DbContextOptions<BusinessDbContext> options) : base(options)
        {

        }
        public DbSet<Account> Account { get; set; }
        public DbSet<Business> Business { get; set; }
        public DbSet<ToDoList> ToDoList { get; set; }
        public DbSet<SignIn> SignIn { get; set; }
        public DbSet<GameScore> GameScore { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "助理" },
                new Role { Id = 2, RoleName = "普通用户" }
            );
        }
    }
}
