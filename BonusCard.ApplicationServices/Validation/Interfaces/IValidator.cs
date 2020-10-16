
namespace BonusCardManager.ApplicationServices.Validation.Interfaces
{
    public interface IValidator<T>
    {
        string Validate(T item);
    }
}
