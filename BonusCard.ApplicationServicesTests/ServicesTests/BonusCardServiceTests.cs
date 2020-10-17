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
                new BonusCard {Id = 1, Balance = 50, ExpirationUTCDate = DateTime.Now, Customer = fakeCustomers[0]},
                new BonusCard {Id = 2, Balance = 0, ExpirationUTCDate = DateTime.Now.AddDays(1), Customer = fakeCustomers[1]},
                new BonusCard {Id = 3, Balance = -10, ExpirationUTCDate = DateTime.Now.AddDays(-1), Customer = fakeCustomers[2]}
            };

            fakeCustomers[0].BonusCard = fakeBonusCards[0];
            fakeCustomers[1].BonusCard = fakeBonusCards[1];
            fakeCustomers[2].BonusCard = fakeBonusCards[2];

            mockBonusCardRepository.Setup(m => m.GetAll())
                          .Returns(fakeBonusCards.AsQueryable);
            mockBonusCardRepository.Setup(m => m.Get(It.IsAny<int>()))
                          .Returns<int>(id => fakeBonusCards.Single(t => t.Id == id));
            mockBonusCardRepository.Setup(r => r.Create(It.IsAny<BonusCard>()))
                          .Callback<BonusCard>(t => fakeBonusCards.Add(t));

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
        public void CreateBonusCard_CorrectBonusCardWithCustomer3_CustomerId3()
        {
            //Arrange
            var bonusCard = new BonusCardDto
            {
                Id = 5,
                Balance = 0,
                ExpirationUTCDate = DateTime.Now.AddDays(1),
                CustomerId = 3
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
            var phoneNumber = fakeCustomers[0].PhoneNumber;
            var expected = fakeCustomers[0].BonusCard.Id;

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
    }
}
