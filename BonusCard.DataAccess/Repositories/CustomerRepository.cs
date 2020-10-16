using BonusCardManager.DataAccess.Entities;
using BonusCardManager.DataAccess.Interfaces;
using System.Linq;

namespace BonusCardManager.DataAccess.Repositories
{
    class CustomerRepository : IRepository<Customer, int>
    {
        #region Private Members

        private readonly DataContext dataContext;

        #endregion

        public CustomerRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Create(Customer item)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Customer Get(int id)
        {
            return dataContext.Customers.Find(id);
        }

        public IQueryable<Customer> GetAll()
        {
            return dataContext.Customers;
        }

        public void Update(Customer item)
        {
            throw new System.NotImplementedException();
        }
    }
}
