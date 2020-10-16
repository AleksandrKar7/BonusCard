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

        public DataContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Customer>()
                .HasOne(u => u.BonusCard)
                .WithOne(p => p.Customer)
                .HasForeignKey<BonusCard>(p => p.Customer);

            modelBuilder.Entity<BonusCard>()
                .HasIndex(u => u.Number)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
