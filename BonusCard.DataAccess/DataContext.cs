using BonusCardManager.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BonusCardManager.DataAccess
{
    public class DataContext : DbContext
    {
        #region DbSets

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<BonusCard> BonusCards { get; set; }

        #endregion

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Customer>()
                .HasOne(c => c.BonusCard)
                .WithOne(b => b.Customer)
                .HasForeignKey<BonusCard>(b => b.CustomerId);

            modelBuilder.Entity<BonusCard>()
                .HasIndex(u => u.Number)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
