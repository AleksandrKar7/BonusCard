using BonusCardManager.ApplicationServices.DTOs;
using BonusCardManager.ApplicationServices.Services.Interfaces;
using BonusCardManager.DataAccess.Entities;
using BonusCardManager.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BonusCardManager.ApplicationServices.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork unitOfWork;
                private readonly MapperService mapper;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            mapper = new MapperService();
        }
        public IEnumerable<CustomerDto> GetNonCardCustomers()
        {
            var nonCardCustomers = unitOfWork.Customers.GetAll().Where(c => c.BonusCard == null).AsEnumerable();

            var nonCardCustomerDtos = mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerDto>>(nonCardCustomers);

            return nonCardCustomerDtos;
        }
    }
}
