
namespace BonusCardManager.WpfUI.Validation.Interfaces
{
    interface IValidator<T>
    {
        bool IsValid(T item);
    }
}
