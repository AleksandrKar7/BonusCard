﻿using BonusCardManager.ApplicationServices.DTOs;
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

        public void WriteOffBalance(int cardId, decimal price)
        {
            throw new NotImplementedException();
        }
    }
}
