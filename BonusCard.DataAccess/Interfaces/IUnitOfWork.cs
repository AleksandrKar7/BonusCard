using BonusCardManager.DataAccess.Entities;

namespace BonusCardManager.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<BonusCard, int> BonusCards { get; }
        IRepository<Customer, int> Customers { get; }

        void Save();
    }
}
