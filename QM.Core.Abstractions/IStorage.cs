using QM.Models.Abstractions;

namespace QM.Core.Abstractions
{
    public interface IStorage
    {
        Task SaveAsync<TRegistrationModel>(TRegistrationModel registrationModel) where TRegistrationModel : IRegistrationModel;

    }
}