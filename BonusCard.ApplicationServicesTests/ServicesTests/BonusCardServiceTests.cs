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

        #endregion

        #region Constructor

        public BonusCardServiceTests()
        {
            var mockRepository = new Mock<IRepository<BonusCard, int>>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            fakeBonusCards = new List<BonusCard>()
            {
                new BonusCard {Id = 1, Balance = 50, ExpirationUTCDate = DateTime.Now},
                new BonusCard {Id = 2, Balance = 0, ExpirationUTCDate = DateTime.Now.AddDays(1)},
                new BonusCard {Id = 3, Balance = -10, ExpirationUTCDate = DateTime.Now.AddDays(-1)}
            };

            mockRepository.Setup(m => m.GetAll())
                          .Returns(fakeBonusCards.AsQueryable);
            mockRepository.Setup(m => m.Get(It.IsAny<int>()))
                          .Returns<int>(id => fakeBonusCards.Single(t => t.Id == id));
            mockRepository.Setup(r => r.Create(It.IsAny<BonusCard>()))
                          .Callback<BonusCard>(t => fakeBonusCards.Add(t));

            mockUnitOfWork.Setup(m => m.BonusCards)
                          .Returns(mockRepository.Object);

            bonusCardService = new BonusCardService(mockUnitOfWork.Object);
        }

        #endregion

        #region Create

        [Fact]
        public void Create_CorrectBonusCard_ShouldBeNotNull()
        {
            //Arrange
            var bonusCard = new BonusCard
            {
                Id = 5,
                Balance = 0,
                ExpirationUTCDate = DateTime.Now.AddDays(1) 
            };

            //Act
            bonusCardService.Create(bonusCard);
            var actual = fakeBonusCards.Last();

            //Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void Create_CorrectBonusCard_EqualIds()
        {
            //Arrange
            var bonusCard = new BonusCard
            {
                Id = 5,
                Balance = 0,
                ExpirationUTCDate = DateTime.Now.AddDays(1)
            };

            //Act
            bonusCardService.Create(bonusCard);
            var actual = fakeBonusCards.Last();

            //Assert
            Assert.Equal(bonusCard.Id, actual.Id);
        }

        [Fact]
        public void Create_IncorrectExpirationDate_ArgumentException()
        {
            //Arrange
            var bonusCard = new BonusCard
            {
                Id = 5,
                Balance = 0,
                ExpirationUTCDate = DateTime.Now.AddDays(-1)
            };

            //Act
            
            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.Create(bonusCard));
        }

        [Fact]
        public void Create_IncorrectExpirationDate_CorrectExceptionMessage()
        {
            //Arrange
            var bonusCard = new BonusCard
            {
                Id = 5,
                Balance = 0,
                ExpirationUTCDate = DateTime.Now.AddDays(-1)
            };
            var expected = "Expiration date cannot be less than the current date";

            //Act
            var actual = Record.Exception(() => bonusCardService.Create(bonusCard)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_NegativeBalance_ArgumentException()
        {
            //Arrange
            var bonusCard = new BonusCard
            {
                Id = 5,
                Balance = -50,
                ExpirationUTCDate = DateTime.Now.AddDays(1)
            };

            //Act

            //Assert
            Assert.Throws<ArgumentException>(() => bonusCardService.Create(bonusCard));
        }

        [Fact]
        public void Create_NegativeBalance_CorrectExceptionMessage()
        {
            //Arrange
            var bonusCard = new BonusCard
            {
                Id = 5,
                Balance = -50,
                ExpirationUTCDate = DateTime.Now.AddDays(1)
            };
            var expected = "Balance cannot be less than zero";

            //Act
            var actual = Record.Exception(() => bonusCardService.Create(bonusCard)).Message.Trim();

            //Assert
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
