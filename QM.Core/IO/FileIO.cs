using Newtonsoft.Json;
using QM.Models.Abstractions;
using QM.Core.Extensions;

namespace QM.Core.IO
{
    public class FileIO
    {
        private readonly string _basePath;
        
        public FileIO(string? basePath = null)
        {
            this._basePath = basePath ?? Directory.GetCurrentDirectory();
        }

        public async Task SaveAsync<TRegistrationModel>(TRegistrationModel registrationModel)
                where TRegistrationModel : IRegistrationModel
        {            
            var fileName = this.GetFileName(registrationModel.CreatedAt);
            var filePath = Path.Combine(this._basePath, fileName);
            var json = JsonConvert.SerializeObject(registrationModel);            
            await File.AppendAllTextAsync(filePath, json + Environment.NewLine);
        }
        
        private string GetFileName(DateTime dateTime) {
            var fileName = $"registration_{dateTime.GetDefaultFormat()}.log";
            return fileName;
        }
    }
}
