using QM.Models.Abstractions;

namespace QM.Core.IO
{
    public interface IFileIO
    {
        Task SaveAsync<TRegistrationModel>(TRegistrationModel registrationModel) where TRegistrationModel : IRegistrationModel;
    }
}