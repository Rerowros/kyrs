using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace kyrs.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Currency> currency { get; set; } = null!; 
        public DbSet<Transaction> Transactions { get; set; }
        
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user=root;password=Rerowros;database=csharp;",
                new MySqlServerVersion(new Version(8, 0, 39)),
                mySqlOptions => mySqlOptions
                .EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
                .CommandTimeout(30));
        }

        

    }
    
}