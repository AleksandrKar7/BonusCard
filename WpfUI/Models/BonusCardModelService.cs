using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BonusCardManager.WpfUI.Models
{
    class BonusCardModelService
    {
        public class BonusCardSearchResult
        {
            public HttpStatusCode ResponseCode { get; set; }
            public BonusCardModel BonusCard { get; set; }
        }

        public static async Task<BonusCardSearchResult> GetBonusCardByCardNumber(int cardNumber)
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

                        return new BonusCardSearchResult
                        {
                            ResponseCode = response.StatusCode,
                            BonusCard = bonusCard
                        };
                    }

                    return new BonusCardSearchResult { ResponseCode = response.StatusCode };
                }
            }
        }

        public static async Task<BonusCardSearchResult> GetBonusCardByPhoneNumber(string phoneNumber)
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

                        return new BonusCardSearchResult
                        {
                            ResponseCode = response.StatusCode,
                            BonusCard = bonusCard
                        };
                    }

                    return new BonusCardSearchResult { ResponseCode = response.StatusCode };
                }
            }
        }

        public static async Task<HttpStatusCode> AccrualBalanceAsync(int cardId, decimal amount)
        {
            using (var httpClient = new HttpClient())
            {
                var request = "https://localhost:44389/api/BonusCard/" + cardId + "/Accrual/" + amount;

                using (var response = await httpClient.PutAsync(request, null))
                {
                    return response.StatusCode;
                }
            }
        }

        public static async Task<HttpStatusCode> WriteOffBalanceAsync(int cardId, decimal amount)
        {
            using (var httpClient = new HttpClient())
            {
                var request = "https://localhost:44389/api/BonusCard/" + cardId + "/WriteOff/" + amount;

                using (var response = await httpClient.PutAsync(request, null))
                {
                    return response.StatusCode;
                }
            }
        }
    }
}
