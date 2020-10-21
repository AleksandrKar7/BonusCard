using BonusCardManager.WpfUI.Models;
using System.Threading.Tasks;

namespace BonusCardManager.WpfUI.Services.Interfaces
{
    interface IBonusCardService
    {
        Task<BonusCardModel> GetBonusCardByCardNumber(int cardNumber);

        Task<BonusCardModel> GetBonusCardByPhoneNumber(string phoneNumber);

        Task<BonusCardModel> CreateBonusCard(BonusCardModel bonusCard);

        Task<bool> AccrualBalanceAsync(int cardId, decimal amount);

        Task<bool> WriteOffBalanceAsync(int cardId, decimal amount);
    }
}
