using BonusCardManager.ApplicationServices.DTOs;
using BonusCardManager.ApplicationServices.Services.Interfaces;
using BonusCardManager.ApplicationServices.Validation;
using BonusCardManager.ApplicationServices.Validation.Interfaces;
using BonusCardManager.DataAccess.Entities;
using BonusCardManager.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BonusCardManager.ApplicationServices.Services
{
    public class BonusCardService : IBonusCardService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IValidator<BonusCardDto> validator;
        private readonly MapperService mapper;

        public BonusCardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            validator = new BonusCardValidator();
            mapper = new MapperService();
        }

        public void CreateBonusCard(BonusCardDto bonusCardDto)
        {
            var errors = validator.Validate(bonusCardDto);
            if (!String.IsNullOrWhiteSpace(errors))
            {
                throw new ArgumentException(errors);
            }

            var bonusCard = mapper.Map<BonusCardDto, BonusCard>(bonusCardDto);

            bonusCard.Customer = unitOfWork.Customers.Get(bonusCardDto.CustomerId);
            if(bonusCard.Customer == null)
            {
                throw new ArgumentException("Customer not found");
            }

            bonusCard.Number = NumberRandomizer.GetUniqueNumber(
                unitOfWork.BonusCards.GetAll()
                                     .Select(x => x.Number)
                                     .AsEnumerable()
            );

            unitOfWork.BonusCards.Create(bonusCard);
            unitOfWork.Save();
        }

        public BonusCardDto GetBonusCard(int cardNumber)
        {
            if (cardNumber <= 0)
            {
                throw new ArgumentException("cardNumber mast be above zero");
            }

            var bonusCard = unitOfWork.BonusCards.GetAll()
                                                 .Where(c => c.Number == cardNumber)
                                                 .Include(c => c.Customer)
                                                 .FirstOrDefault();

            var bonusCardDto = mapper.Map<BonusCard, BonusCardDto>(bonusCard);

            return bonusCardDto;
        }

        public BonusCardDto GetBonusCard(string customerPhoneNumber)
        {
            if(String.IsNullOrWhiteSpace(customerPhoneNumber))
            {
                throw new ArgumentException("customerPhoneNumber can not be empty");
            }

            var bonusCard = unitOfWork.Customers.GetAll()
                                                .Where(p => p.PhoneNumber == customerPhoneNumber)
                                                .Select(b => b.BonusCard)
                                                .Include(c => c.Customer)
                                                .FirstOrDefault();

            var bonusCardDto = mapper.Map<BonusCard, BonusCardDto>(bonusCard);

            return bonusCardDto;
        }

        public void AccrualBalance(int cardId, decimal amount)
        {
            if (cardId <= 0)
            {
                throw new ArgumentException("cardId mast be above zero");
            }
            if (amount <= 0)
            {
                throw new ArgumentException("amount mast be above zero");
            }

            var bonusCard = unitOfWork.BonusCards.Get(cardId);
            if (bonusCard.ExpirationUTCDate.Date < DateTime.Now.Date)
            {
                throw new ArgumentException("bonus card is expired");
            }

            bonusCard.Balance += amount;

            unitOfWork.BonusCards.Update(bonusCard);
            unitOfWork.Save();
        }

        public void WriteOffBalance(int cardId, decimal amount)
        {
            if (cardId <= 0)
            {
                throw new ArgumentException("cardId mast be above zero");
            }
            if (amount <= 0)
            {
                throw new ArgumentException("amount mast be above zero");
            }

            var bonusCard = unitOfWork.BonusCards.Get(cardId);
            if (bonusCard.ExpirationUTCDate.Date < DateTime.Now.Date)
            {
                throw new ArgumentException("bonus card is expired");
            }

            if(bonusCard.Balance - amount < 0)
            {
                throw new ArgumentException("not enough money on the bonus card");
            }
            bonusCard.Balance -= amount;

            unitOfWork.BonusCards.Update(bonusCard);
            unitOfWork.Save();
        }
    }
}
