using QM.Models.Abstractions;

namespace QM.Mapper.Abstractions
{
    public interface IRepository
    {
        Task SaveChangesAsync<TRegistrationModel>(TRegistrationModel registrationModel) where TRegistrationModel : IRegistrationModel;
    }
}