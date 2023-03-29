using Microsoft.Extensions.Configuration;

namespace QM.Core.Helper
{
    public class ParameterHelper
    {
        private readonly IConfiguration _config;
        private const string ConfigurationFile = "appsettings.json";
        public ParameterHelper()
        {
            _config = new ConfigurationBuilder()                                
                .AddEnvironmentVariables()
                .AddJsonFile(ConfigurationFile, optional: true)
                .Build();
        }

        public string GetParameter(string key)
        {
            var value = _config[key];
            if (value != null)
            {
                return value;
            }

            value = Environment.GetEnvironmentVariable(key);
            return value;
        }
    }
}
