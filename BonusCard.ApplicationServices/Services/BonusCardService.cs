using BonusCardManager.ApplicationServices.DTOs;
using BonusCardManager.ApplicationServices.Services.Interfaces;
using BonusCardManager.ApplicationServices.Validation;
using BonusCardManager.ApplicationServices.Validation.Interfaces;
using BonusCardManager.DataAccess.Entities;
using BonusCardManager.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
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

        public BonusCardDto CreateBonusCard(BonusCardDto bonusCardDto)
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
            if (bonusCard.Customer.BonusCard != null)
            {
                throw new ArgumentException("Customer already has a card");
            }

            bonusCard.Number = GetUniqueNumber();

            unitOfWork.BonusCards.Create(bonusCard);
            unitOfWork.Save();

            return mapper.Map<BonusCard, BonusCardDto>(bonusCard);
        }

        private int GetUniqueNumber(int clasterSize = 1000)
        {
            var incompleteClusters = unitOfWork.BonusCards.GetAll()
                                                          .GroupBy(x => x.Number / clasterSize)
                                                          .Where(c => c.Count() < clasterSize)
                                                          .Select(k => k.Key)
                                                          .ToList();

            var clasterNumber = incompleteClusters[new Random().Next(0, incompleteClusters.Count)];

            var minNumber = clasterNumber * clasterSize;
            var maxNumber = (clasterNumber + 1) * clasterSize;

            var usedNumbers = unitOfWork.BonusCards.GetAll()
                                                   .Select(x => x.Number)
                                                   .Where(x => (minNumber < x && x < maxNumber))
                                                   .ToList();

            return NumberRandomizer.GetUniqueNumber(usedNumbers, maxNumber, minNumber);
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

            var bonusCard = unitOfWork.BonusCards.GetAll()
                                    .Include(c => c.Customer)
                                    .Where(p => p.Customer.PhoneNumber == customerPhoneNumber)
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
            if(bonusCard == null)
            {
                throw new ArgumentException("bonus card not found");
            }
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
            if (bonusCard == null)
            {
                throw new ArgumentException("bonus card not found");
            }
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
