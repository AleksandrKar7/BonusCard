using BonusCardManager.ApplicationServices.DTOs;
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
        private readonly IValidator<BonusCardDto> validator;
        private readonly MapperService mapper;


        public BonusCardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            validator = new BonusCardValidator();
            mapper = new MapperService();
        }

        public void AccrualBalance(int cardId, decimal price)
        {
            throw new NotImplementedException();
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
