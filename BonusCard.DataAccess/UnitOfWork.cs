using BonusCardManager.DataAccess.Entities;
using BonusCardManager.DataAccess.Interfaces;
using BonusCardManager.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BonusCardManager.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DataContext dataContext;

        private IRepository<BonusCard, int> bonusCardRepository;
        private IRepository<Customer, int> customerRepository;

        public IRepository<BonusCard, int> BonusCards 
        {
            get 
            {
                if (bonusCardRepository == null)
                    bonusCardRepository = new BonusCardRepository(dataContext);
                return bonusCardRepository;
            }
        }
        public IRepository<Customer, int> Customers 
        { 
            get 
            {
                if (customerRepository == null)
                    customerRepository = new CustomerRepository(dataContext);
                return customerRepository;
            } 
        }

        public UnitOfWork(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Save()
        {
            dataContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dataContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
