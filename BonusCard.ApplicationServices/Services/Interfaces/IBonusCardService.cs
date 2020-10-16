using BonusCardManager.DataAccess.Entities;

namespace BonusCardManager.ApplicationServices.Services.Interfaces
{
    public interface IBonusCardService
    {
        BonusCard GetBonusCard(string customerPhone);
        BonusCard GetBonusCard(int cardNumber);
        
        void CreateBonusCard(BonusCard bonusCard);

        void WriteOffBalance(int cardId, decimal amount);
        void AccrualBalance(int cardId, decimal amount);
    }
}
