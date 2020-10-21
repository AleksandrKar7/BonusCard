using BonusCardManager.ApplicationServices.DTOs;
using System.Collections.Generic;

namespace BonusCardManager.ApplicationServices.Services.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<CustomerDto> GetNonCardCustomers();
    }
}
