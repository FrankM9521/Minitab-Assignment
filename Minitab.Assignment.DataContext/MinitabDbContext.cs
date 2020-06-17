using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minitab.Assignment.Entities;

namespace Minitab.Assignment.DataContext
{
    public class MinitabDbContext : DbContext
    {
        public MinitabDbContext(DbContextOptions<MinitabDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<AddressEntity> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
