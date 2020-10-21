using BonusCardManager.WpfUI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BonusCardManager.WpfUI.Services.Interfaces
{
    interface ICustomerService
    {
        Task<IEnumerable<CustomerModel>> GetNonCardCustomers();
    }
}
