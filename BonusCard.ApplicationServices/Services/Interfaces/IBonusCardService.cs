using BonusCardManager.ApplicationServices.DTOs;
using BonusCardManager.DataAccess.Entities;

namespace BonusCardManager.ApplicationServices.Services.Interfaces
{
    public interface IBonusCardService
    {
        BonusCardDto GetBonusCard(string customerPhoneNumber);
        BonusCardDto GetBonusCard(int cardNumber);
        
        void CreateBonusCard(BonusCardDto bonusCard);

        void WriteOffBalance(int cardId, decimal amount);
        void AccrualBalance(int cardId, decimal amount);
    }
}
