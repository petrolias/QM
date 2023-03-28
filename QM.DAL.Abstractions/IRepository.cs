using QM.Models.Abstractions;

namespace QM.DAL.Abstractions
{
    public interface IRepository
    {
        Task SaveChangesAsync<TRegistrationModel>(TRegistrationModel registrationModel) where TRegistrationModel : IRegistrationModel;
    }
}