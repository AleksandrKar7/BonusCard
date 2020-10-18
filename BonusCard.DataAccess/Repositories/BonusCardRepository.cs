using BonusCardManager.DataAccess.Entities;
using BonusCardManager.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BonusCardManager.DataAccess.Repositories
{
    class BonusCardRepository : IRepository<BonusCard, int>
    {
        #region Private Members

        private readonly DataContext dataContext;

        #endregion

        public BonusCardRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Create(BonusCard item)
        {
            dataContext.Add(item);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public BonusCard Get(int id)
        {
            return dataContext.BonusCards.Include(c => c.Customer)
                                         .FirstOrDefault(b => b.Id == id);
        }

        public IQueryable<BonusCard> GetAll()
        {
            return dataContext.BonusCards;
        }

        public void Update(BonusCard item)
        {
            throw new NotImplementedException();
        }
    }
}
