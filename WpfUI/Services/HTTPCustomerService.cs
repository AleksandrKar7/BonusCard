using BonusCardManager.WpfUI.Models;
using BonusCardManager.WpfUI.Services.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BonusCardManager.WpfUI.Services
{
    class HTTPCustomerService : ICustomerService
    {
        public async Task<IEnumerable<CustomerModel>> GetNonCardCustomers()
        {
            using (var httpClient = new HttpClient())
            {
                var request = "https://localhost:44389/api/Customers/NonCard";
                using (var response = await httpClient.GetAsync(request))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var customers = JsonConvert.DeserializeObject<IEnumerable<CustomerModel>>(apiResponse);

                        return customers;
                    }

                    return null;
                }
            }
        }
    }
}
