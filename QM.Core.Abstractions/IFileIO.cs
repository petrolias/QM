using QM.Models.Abstractions;

namespace QM.Core.IO
{
    public interface IFileIO
    {
        Task AddFile<TRegistrationModel>(TRegistrationModel registrationModel) where TRegistrationModel : IRegistrationModel;
    }
}