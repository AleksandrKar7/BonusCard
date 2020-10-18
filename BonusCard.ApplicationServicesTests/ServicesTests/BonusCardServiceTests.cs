using BonusCardManager.ApplicationServices.DTOs;
using BonusCardManager.ApplicationServices.Services;
using BonusCardManager.DataAccess.Entities;
using BonusCardManager.DataAccess.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BonusCardManager.ApplicationServicesTests.ServicesTests
{
    public class BonusCardServiceTests
    {
        #region Private fields

        private readonly BonusCardService bonusCardService;
        private readonly IList<BonusCard> fakeBonusCards;
        private readonly IList<Customer> fakeCustomers;

        #endregion



        #region Constructor

        public BonusCardServiceTests()
        {
            var mockBonusCardRepository = new Mock<IRepository<BonusCard, int>>();
            var mockCustomerRepository = new Mock<IRepository<Customer, int>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();


            fakeCustomers = new List<Customer>()
            {
                new Customer {Id = 1, FullName = "Etiam sapien enim", PhoneNumber = "8805553535"},
                new Customer {Id = 2, FullName = "Molestie eget risus", PhoneNumber = "8805553534"},
                new Customer {Id = 3, FullName = "Non dignissim tristique", PhoneNumber = "8805553533"}
            };

            fakeBonusCards = new List<BonusCard>()
            {
                new BonusCard {Id = 1, Balance = 50, Number = 1, ExpirationUTCDate = DateTime.Now, Customer = fakeCustomers[0]},
                new BonusCard {Id = 2, Balance = 0, Number = 2, ExpirationUTCDate = DateTime.Now.AddDays(1), Customer = fakeCustomers[1]},
                new BonusCard {Id = 3, Balance = -10, Number = 3, ExpirationUTCDate = DateTime.Now.AddDays(-1), Customer = fakeCustomers[2]}
            };

            fakeCustomers[2].BonusCard = fakeBonusCards[2];

            mockBonusCardRepository.Setup(m => m.GetAll())
                          .Returns(fakeBonusCards.AsQueryable);
            mockBonusCardRepository.Setup(m => m.Get(It.IsAny<int>()))
                          .Returns<int>(id => fakeBonusCards.FirstOrDefault(t => t.Id == id));
            mockBonusCardRepository.Setup(r => r.Create(It.IsAny<BonusCard>()))
                          .Callback<BonusCard>(t => fakeBonusCards.Add(t));
            mockBonusCardRepository.Setup(r => r.Update(It.IsAny<BonusCard>()))
                          .Callback<BonusCard>(t => fakeBonusCards.Insert(fakeBonusCards.IndexOf(fakeBonusCards.Where(i => i.Id == t.Id).First()), t));

            mockCustomerRepository.Setup(m => m.GetAll())
                                  .Returns(fakeCustomers.AsQueryable);
            mockCustomerRepository.Setup(m => m.Get(It.IsAny<int>()))
                                  .Returns<int>(id => fakeCustomers.FirstOrDefault(t => t.Id == id));


            mockUnitOfWork.Setup(m => m.BonusCards)
                          .Returns(mockBonusCardRepository.Object);
            mockUnitOfWork.Setup(m => m.Customers)
                          .Returns(mockCustomerRepository.Object);

            bonusCardService = new BonusCardService(mockUnitOfWork.Object);
        }

        #endregion



        #region CreateBonusCard tests 

        #region Positive cases

        [Fact]
        public void CreateBonusCard_CorrectBonusCard_ShouldBeNotNull()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = 0,
                ExpirationUTCDate = DateTime.Now.AddDays(1),
                CustomerId = 1
            };

            //Act
            bonusCardService.CreateBonusCard(bonusCard);
            var actual = fakeBonusCards.Last();

            //Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void CreateBonusCard_CorrectBonusCard_EqualIds()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = 0,
                ExpirationUTCDate = DateTime.Now.AddDays(1),
                CustomerId = 1
            };

            //Act
            bonusCardService.CreateBonusCard(bonusCard);
            var actual = fakeBonusCards.Last();

            //Assert
            Assert.Equal(bonusCard.Id, actual.Id);
        }

        [Fact]
        public void CreateBonusCard_CorrectBonusCardWithCustomer2_CustomerId2()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = 0,
                ExpirationUTCDate = DateTime.Now.AddDays(1),
                CustomerId = 2
            };

            //Act
            bonusCardService.CreateBonusCard(bonusCard);
            var actual = fakeBonusCards.Last();

            //Assert
            Assert.Equal(bonusCard.CustomerId, actual.Customer.Id);
        }

        #endregion Positive cases

        #region Negative cases

        [Fact]
        public void CreateBonusCard_Null_ArgumentException()
        {
            //Arrange
            BonusCardDto bonusCard = null;

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.CreateBonusCard(bonusCard));
        }

        [Fact]
        public void CreateBonusCard_Null_CorrectExceptionMessage()
        {
            //Arrange
            BonusCardDto bonusCard = null;
            var expected = "bonusCard can not be null";

            //Act
            var actual = Record.Exception(() => bonusCardService.CreateBonusCard(bonusCard)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateBonusCard_IncorrectExpirationDate_ArgumentException()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = 0,
                ExpirationUTCDate = DateTime.Now.AddDays(-1),
                CustomerId = 1
            };

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.CreateBonusCard(bonusCard));
        }

        [Fact]
        public void CreateBonusCard_IncorrectExpirationDate_CorrectExceptionMessage()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = 0,
                ExpirationUTCDate = DateTime.Now.AddDays(-1),
                CustomerId = 1
            };
            var expected = "Expiration date cannot be less than the current date";

            //Act
            var actual = Record.Exception(() => bonusCardService.CreateBonusCard(bonusCard)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateBonusCard_NegativeBalance_ArgumentException()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = -50,
                ExpirationUTCDate = DateTime.Now.AddDays(1),
                CustomerId = 1
            };

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.CreateBonusCard(bonusCard));
        }

        [Fact]
        public void CreateBonusCard_NegativeBalance_CorrectExceptionMessage()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = -50,
                ExpirationUTCDate = DateTime.Now.AddDays(1),
                CustomerId = 1
            };
            var expected = "Balance cannot be less than zero";

            //Act
            var actual = Record.Exception(() => bonusCardService.CreateBonusCard(bonusCard)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateBonusCard_NegativeCustomerId_ArgumentException()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = -50,
                ExpirationUTCDate = DateTime.Now.AddDays(1),
                CustomerId = -1
            };

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.CreateBonusCard(bonusCard));
        }

        [Fact]
        public void CreateBonusCard_NegativeCustomerId_CorrectExceptionMessage()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = 50,
                ExpirationUTCDate = DateTime.Now.AddDays(1),
                CustomerId = -1
            };
            var expected = "CustomerId must be above zero";

            //Act
            var actual = Record.Exception(() => bonusCardService.CreateBonusCard(bonusCard)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateBonusCard_NotRealCustomerId_ArgumentException()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = 50,
                ExpirationUTCDate = DateTime.Now.AddDays(1),
                CustomerId = Int32.MaxValue
            };

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.CreateBonusCard(bonusCard));
        }

        [Fact]
        public void CreateBonusCard_NotRealCustomerId_CorrectExceptionMessage()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = 50,
                ExpirationUTCDate = DateTime.Now.AddDays(1),
                CustomerId = Int32.MaxValue
            };
            var expected = "Customer not found";

            //Act
            var actual = Record.Exception(() => bonusCardService.CreateBonusCard(bonusCard)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateBonusCard_CustomeHasACard_ArgumentException()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = 50,
                ExpirationUTCDate = DateTime.Now.AddDays(1),
                CustomerId = 3
            };

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.CreateBonusCard(bonusCard));
        }

        [Fact]
        public void CreateBonusCard_ustomeHasACard_CorrectExceptionMessage()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = 50,
                ExpirationUTCDate = DateTime.Now.AddDays(1),
                CustomerId = 3
            };
            var expected = "Customer already has a card";

            //Act
            var actual = Record.Exception(() => bonusCardService.CreateBonusCard(bonusCard)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        #endregion Negative cases

        #endregion CreateBonusCard tests 



        #region GetBonusCard by phone tests 

        #region Positive cases

        [Fact]
        public void GetBonusCard_CorrectCustomerPhoneNumber_ShouldBeNotNull()
        {
            //Arrange
            var phoneNumber = fakeCustomers[0].PhoneNumber;

            //Act
            var bonusCard = bonusCardService.GetBonusCard(phoneNumber);

            //Assert
            Assert.NotNull(bonusCard);
        }

        [Fact]
        public void GetBonusCard_CorrectCustomerPhoneNumber_CorrectBonusCardId()
        {
            //Arrange
            var phoneNumber = fakeCustomers[2].PhoneNumber;
            var expected = fakeCustomers[2].BonusCard.Id;

            //Act
            var actual = bonusCardService.GetBonusCard(phoneNumber);

            //Assert
            Assert.Equal(expected, actual.Id);
        }

        #endregion Positive cases

        #region Negative cases

        [Fact]
        public void GetBonusCard_NullPhoneNumber_ArgumentException()
        {
            //Arrange
            string phoneNumber = null;

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.GetBonusCard(phoneNumber));
        }

        [Fact]
        public void GetBonusCard_NullPhoneNumber_CorrectExceptionMessage()
        {
            //Arrange
            string phoneNumber = null;
            var expected = "customerPhoneNumber can not be empty";

            //Act
            var actual = Record.Exception(() => bonusCardService.GetBonusCard(phoneNumber)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetBonusCard_NotRealPhoneNumber_Null()
        {
            //Arrange
            string phoneNumber = "ItIsNotAPhone";
            BonusCardDto expected = null;

            //Act
            var actual = bonusCardService.GetBonusCard(phoneNumber);


            //Assert
            Assert.Equal(expected, actual);
        }

        #endregion Negative cases

        #endregion GetBonusCard by phone tests 



        #region GetBonusCard by card number tests 

        #region Positive cases

        [Fact]
        public void GetBonusCard_CorrectCardNumber_ShouldBeNotNull()
        {
            //Arrange
            var cardNumber = fakeBonusCards[0].Number;

            //Act
            var bonusCard = bonusCardService.GetBonusCard(cardNumber);

            //Assert
            Assert.NotNull(bonusCard);
        }

        [Fact]
        public void GetBonusCard_CorrectCardNumber_CorrectBonusCardId()
        {
            //Arrange
            var cardNumber = fakeBonusCards[0].Number;
            var expected = fakeBonusCards[0].Id;

            //Act
            var actual = bonusCardService.GetBonusCard(cardNumber);

            //Assert
            Assert.Equal(expected, actual.Id);
        }

        #endregion Positive cases

        #region Negative cases

        [Fact]
        public void GetBonusCard_CardNumberLessZero_ArgumentException()
        {
            //Arrange
            var cardNumber = -1;

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.GetBonusCard(cardNumber));
        }

        [Fact]
        public void GetBonusCard_CardNumberLessZero_CorrectExceptionMessage()
        {
            //Arrange
            var cardNumber = -1;
            var expected = "cardNumber mast be above zero";

            //Act
            var actual = Record.Exception(() => bonusCardService.GetBonusCard(cardNumber)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetBonusCard_NonExistentCardNumber_Null()
        {
            //Arrange
            var cardNumber = Int32.MaxValue;
            BonusCardDto expected = null;

            //Act
            var actual = bonusCardService.GetBonusCard(cardNumber);

            //Assert
            Assert.Equal(expected, actual);
        }

        #endregion Negative cases

        #endregion GetBonusCard by phone tests 



        #region AccrualBalance tests

        #region Positive cases

        [Fact]
        public void AccrualBalance_CorrectCardIdCorrectAmount_BalanceIncreased()
        {
            //Arrange
            var card = fakeBonusCards[0];
            var amount = 20.00M;
            var expected = card.Balance + amount;

            //Act
            bonusCardService.AccrualBalance(card.Id, amount);
            var actual = card.Balance;

            //Assert
            Assert.Equal(expected, actual);
        }

        #endregion Positive cases

        #region Negative cases

        [Fact]
        public void AccrualBalance_IncorrectCardIdCorrectAmount_ArgumentException()
        {
            //Arrange
            var cardId = -1;
            var amount = 20.00M;


            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.AccrualBalance(cardId, amount));
        }

        [Fact]
        public void AccrualBalance_IncorrectCardIdCorrectAmount_CorrectExceptionMessage()
        {
            //Arrange
            var cardId = -1;
            var amount = 20.00M;
            var expected = "cardId mast be above zero";

            //Act
            var actual = Record.Exception(() => bonusCardService.AccrualBalance(cardId, amount)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AccrualBalance_CorrectCardIdNegativeAmount_ArgumentException()
        {
            //Arrange
            var cardId = fakeBonusCards[0].Id;
            var amount = -20.00M;


            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.AccrualBalance(cardId, amount));
        }

        [Fact]
        public void AccrualBalance_CorrectCardIdNegativeAmount_CorrectExceptionMessage()
        {
            //Arrange
            var cardId = fakeBonusCards[0].Id;
            var amount = -20.00M;
            var expected = "amount mast be above zero";

            //Act
            var actual = Record.Exception(() => bonusCardService.AccrualBalance(cardId, amount)).Message.Trim();
            //Assert

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AccrualBalance_ExpiredCardCorrectAmount_ArgumentException()
        {
            //Arrange
            var cardId = fakeBonusCards[2].Id;
            var amount = 20.00M;


            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.AccrualBalance(cardId, amount));
        }

        [Fact]
        public void AccrualBalance_ExpiredCardCorrectAmount_CorrectExceptionMessage()
        {
            //Arrange
            var cardId = fakeBonusCards[2].Id;
            var amount = 20.00M;
            var expected = "bonus card is expired";

            //Act
            var actual = Record.Exception(() => bonusCardService.AccrualBalance(cardId, amount)).Message.Trim();
            //Assert

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AccrualBalance_NotRealCardCardCorrectAmount_ArgumentException()
        {
            //Arrange
            var cardId = Int32.MaxValue;
            var amount = 20.00M;


            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.AccrualBalance(cardId, amount));
        }

        [Fact]
        public void AccrualBalance_NotRealCardCorrectAmount_CorrectExceptionMessage()
        {
            //Arrange
            var cardId = Int32.MaxValue;
            var amount = 20.00M;
            var expected = "bonus card not found";

            //Act
            var actual = Record.Exception(() => bonusCardService.AccrualBalance(cardId, amount)).Message.Trim();
            //Assert

            Assert.Equal(expected, actual);
        }

        #endregion Negative cases

        #endregion AccrualBalance tests



        #region WriteOffBalance tests

        #region Positive cases

        [Fact]
        public void WriteOffBalance_CorrectCardIdCorrectAmount_BalanceDecreased()
        {
            //Arrange
            var card = fakeBonusCards[0];
            var amount = 20.00M;
            var expected = card.Balance - amount;

            //Act
            bonusCardService.WriteOffBalance(card.Id, amount);
            var actual = card.Balance;

            //Assert
            Assert.Equal(expected, actual);
        }

        #endregion Positive cases

        #region Negative cases

        [Fact]
        public void WriteOffBalance_IncorrectCardIdCorrectAmount_ArgumentException()
        {
            //Arrange
            var cardId = -1;
            var amount = 20.00M;

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.WriteOffBalance(cardId, amount));
        }

        [Fact]
        public void WriteOffBalance_IncorrectCardIdCorrectAmount_CorrectExceptionMessage()
        {
            //Arrange
            var cardId = -1;
            var amount = 20.00M;
            var expected = "cardId mast be above zero";

            //Act
            var actual = Record.Exception(() => bonusCardService.WriteOffBalance(cardId, amount)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteOffBalance_CorrectCardIdNegativeAmount_ArgumentException()
        {
            //Arrange
            var cardId = fakeBonusCards[0].Id;
            var amount = -20.00M;

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.WriteOffBalance(cardId, amount));
        }

        [Fact]
        public void WriteOffBalance_CorrectCardIdNegativeAmount_CorrectExceptionMessage()
        {
            //Arrange
            var cardId = fakeBonusCards[0].Id;
            var amount = -20.00M;
            var expected = "amount mast be above zero";

            //Act
            var actual = Record.Exception(() => bonusCardService.WriteOffBalance(cardId, amount)).Message.Trim();
            
            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteOffBalance_ExpiredCardCorrectAmount_ArgumentException()
        {
            //Arrange
            var cardId = fakeBonusCards[2].Id;
            var amount = 20.00M;

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.WriteOffBalance(cardId, amount));
        }

        [Fact]
        public void WriteOffBalance_ExpiredCardCorrectAmount_CorrectExceptionMessage()
        {
            //Arrange
            var cardId = fakeBonusCards[2].Id;
            var amount = 20.00M;
            var expected = "bonus card is expired";

            //Act
            var actual = Record.Exception(() => bonusCardService.WriteOffBalance(cardId, amount)).Message.Trim();
            //Assert

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteOffBalance_NegativeCardBalanceCorrectAmount_ArgumentException()
        {
            //Arrange
            var cardId = fakeBonusCards[1].Id;
            var amount = 20.00M;

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.WriteOffBalance(cardId, amount));
        }

        [Fact]
        public void WriteOffBalance_NegativeCardBalanceCorrectAmount_CorrectExceptionMessage()
        {
            //Arrange
            var cardId = fakeBonusCards[1].Id;
            var amount = 20.00M;
            var expected = "not enough money on the bonus card";

            //Act
            var actual = Record.Exception(() => bonusCardService.WriteOffBalance(cardId, amount)).Message.Trim();
            
            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteOffBalance_NotRealCardCardCorrectAmount_ArgumentException()
        {
            //Arrange
            var cardId = Int32.MaxValue;
            var amount = 20.00M;


            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.WriteOffBalance(cardId, amount));
        }

        [Fact]
        public void WriteOffBalance_NotRealCardCorrectAmount_CorrectExceptionMessage()
        {
            //Arrange
            var cardId = Int32.MaxValue;
            var amount = 20.00M;
            var expected = "bonus card not found";

            //Act
            var actual = Record.Exception(() => bonusCardService.WriteOffBalance(cardId, amount)).Message.Trim();
            //Assert

            Assert.Equal(expected, actual);
        }

        #endregion Negative cases

        #endregion AccrualBalance tests
    }
}
