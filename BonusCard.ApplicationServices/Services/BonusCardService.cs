using BonusCardManager.ApplicationServices.Services.Interfaces;
using BonusCardManager.ApplicationServices.Validation;
using BonusCardManager.ApplicationServices.Validation.Interfaces;
using BonusCardManager.DataAccess.Entities;
using BonusCardManager.DataAccess.Interfaces;
using System;
using System.Linq;

namespace BonusCardManager.ApplicationServices.Services
{
    public class BonusCardService : IBonusCardService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IValidator<BonusCard> validator;

        public BonusCardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            validator = new BonusCardValidator();
        }

        public void AccrualBalance(int cardId, decimal price)
        {
            throw new NotImplementedException();
        }

        public void Create(BonusCard bonusCard)
        {
            string errors = validator.Validate(bonusCard);
            if (!String.IsNullOrWhiteSpace(errors))
            {
                throw new ArgumentException(errors);
            }

            bonusCard.Number = NumberRandomizer.GetUniqueNumber(
                unitOfWork.BonusCards.GetAll()
                                     .Select(x => x.Number)
                                     .AsEnumerable()
            );
            unitOfWork.BonusCards.Create(bonusCard);

            unitOfWork.Save();
        }

        public void CreateBonusCard(BonusCard bonusCard)
        {
            throw new NotImplementedException();
        }

        public BonusCard GetBonusCard(int cardNumber)
        {
            throw new NotImplementedException();
        }

        public BonusCard GetBonusCard(string customerPhone)
        {
            throw new NotImplementedException();
        }

        public void WriteOffBalance(int cardId, decimal price)
        {
            throw new NotImplementedException();
        }
    }
}
