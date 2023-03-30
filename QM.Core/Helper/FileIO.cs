using Newtonsoft.Json;
using QM.Models.Abstractions;
using QM.Core.Extensions;

namespace QM.Core.Helper
{
    public class FileIO
    {
        private readonly string _basePath;

        public FileIO(string? basePath = null)
        {
            _basePath = basePath ?? Directory.GetCurrentDirectory();
        }

        public async Task AddFile<TRegistrationModel>(TRegistrationModel registrationModel)
                where TRegistrationModel : IRegistrationModel
        {            
            var fileName = GetFileName(registrationModel.Guid, registrationModel.CreatedAt);
            var filePath = Path.Combine(_basePath, fileName);
            var json = JsonConvert.SerializeObject(registrationModel);
            using StreamWriter sw = new(filePath, true);
            await sw.WriteLineAsync(json);
        }

        private string GetFileName(Guid guid, DateTime dateTime)
        {
            var fileName = $"registration_{guid.ToString()}_{dateTime.GetDefaultFormat()}.log";
            return fileName;
        }
    }
}
