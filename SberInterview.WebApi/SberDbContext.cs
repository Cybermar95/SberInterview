using Microsoft.EntityFrameworkCore;
using SberInterview.WebApi.Accounts;
using SberInterview.WebApi.Users;

namespace SberInterview.WebApi
{
    public class SberDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SberDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}