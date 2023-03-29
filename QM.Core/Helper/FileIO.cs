using Newtonsoft.Json;
using QM.Models.Abstractions;
using QM.Core.Extensions;
using QM.Core.Abstractions;
using QM.Core.IO;

namespace QM.Core.Helper
{
    public class FileIO : IStorage, IFileIO
    {
        private readonly string _basePath;

        public FileIO(string? basePath = null)
        {
            _basePath = basePath ?? Directory.GetCurrentDirectory();
        }

        public async Task SaveAsync<TRegistrationModel>(TRegistrationModel registrationModel)
                where TRegistrationModel : IRegistrationModel
        {
            var fileName = GetFileName(registrationModel.CreatedAt);
            var filePath = Path.Combine(_basePath, fileName);
            var json = JsonConvert.SerializeObject(registrationModel);
            await File.AppendAllTextAsync(filePath, json + Environment.NewLine);
        }

        private string GetFileName(DateTime dateTime)
        {
            var fileName = $"registration_{dateTime.GetDefaultFormat()}.log";
            return fileName;
        }
    }
}
