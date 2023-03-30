using QM.Models.Abstractions;

namespace QM.Core.IO
{
    public interface IFileIO
    {
        Task AppendToFileAsync<TRegistrationModel>(TRegistrationModel registrationModel) where TRegistrationModel : IRegistrationModel;
    }
}