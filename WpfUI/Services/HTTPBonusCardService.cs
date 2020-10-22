using BonusCardManager.WpfUI.Models;
using BonusCardManager.WpfUI.Services.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BonusCardManager.WpfUI.Services
{
    class HTTPBonusCardService : IBonusCardService
    {
        public async Task<BonusCardModel> GetBonusCardByCardNumber(int cardNumber)
        {
            using (var httpClient = new HttpClient())
            {
                var request = "https://localhost:44389/api/BonusCard/ByCardNumber/" + cardNumber;
                using (var response = await httpClient.GetAsync(request))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var bonusCard = JsonConvert.DeserializeObject<BonusCardModel>(apiResponse);

                        return bonusCard;
                    }

                    return null;
                }
            }
        }

        public async Task<BonusCardModel> GetBonusCardByPhoneNumber(string phoneNumber)
        {
            using (var httpClient = new HttpClient())
            {
                var request = "https://localhost:44389/api/BonusCard/ByPhoneNumber/" + phoneNumber;
                using (var response = await httpClient.GetAsync(request))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var bonusCard = JsonConvert.DeserializeObject<BonusCardModel>(apiResponse);

                        return bonusCard;
                    }

                    return null;
                }
            }
        }

        public async Task<bool> AccrualBalanceAsync(int cardId, decimal amount)
        {
            using (var httpClient = new HttpClient())
            {
                var request = "https://localhost:44389/api/BonusCard/" + cardId + "/Accrual";

                var body = JsonConvert.SerializeObject(amount);
                using (var response = await httpClient.PutAsync(request, new StringContent(body, Encoding.UTF8, "application/json")))
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
        }

        public async Task<bool> WriteOffBalanceAsync(int cardId, decimal amount)
        {
            using (var httpClient = new HttpClient())
            {
                var request = "https://localhost:44389/api/BonusCard/" + cardId + "/WriteOff";

                var body = JsonConvert.SerializeObject(amount);
                using (var response = await httpClient.PutAsync(request, new StringContent(body, Encoding.UTF8, "application/json")))
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
        }

        public async Task<BonusCardModel> CreateBonusCard(BonusCardModel bonusCard)
        {
            using (var httpClient = new HttpClient())
            {
                var request = "https://localhost:44389/api/BonusCard";
                var body = JsonConvert.SerializeObject(bonusCard);
                using (var response = await httpClient.PostAsync(request, new StringContent(body, Encoding.UTF8, "application/json")))
                {
                    if(response.StatusCode == HttpStatusCode.Created)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var newBonusCard = JsonConvert.DeserializeObject<BonusCardModel>(apiResponse);

                        return newBonusCard;
                    }
                }
            }

            return null;
        }
    }
}
