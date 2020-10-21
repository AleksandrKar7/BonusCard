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
    public class CustomerServiceTests
    {
        #region Private fields

        private readonly CustomerService customerService;
        private readonly IList<BonusCard> fakeBonusCards;
        private readonly IList<Customer> fakeCustomers;

        #endregion



        #region Constructor

        public CustomerServiceTests()
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
            fakeCustomers[1].BonusCard = fakeBonusCards[1];

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
            
            customerService = new CustomerService(mockUnitOfWork.Object);
        }

        #endregion

        #region GetNonCardCustomers

        #region Positive cases

        [Fact]
        public void GetNonCardCustomers_OneNonCardCustomer_NotNull()
        {
            //Arrange

            //Act
            var customers = customerService.GetNonCardCustomers();            

            //Assert
            Assert.NotNull(customers);
        }

        [Fact]
        public void GetNonCardCustomers_OneNonCardCustomer_Length1()
        {
            //Arrange
            var expected = 1;

            //Act
            var customers = customerService.GetNonCardCustomers();
            var actual = customers.Count();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNonCardCustomers_OneNonCardCustomer_EqualCustomerFullName()
        {
            //Arrange
            var expected = fakeCustomers[0].FullName;

            //Act
            var customers = customerService.GetNonCardCustomers();
            var actual = customers.First().FullName;

            //Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #endregion
    }
}
