using Microsoft.Extensions.Configuration;
using QM.Core.Abstractions.Enums;

namespace QM.Core.Helper
{
    public class ParameterHelper
    {
        private readonly IConfiguration _config;
        private const string ConfigurationFile = "appsettings.json";        
        public ParameterHelper(string configurationFile = ConfigurationFile)
        {            
            _config = new ConfigurationBuilder()                                
                .AddEnvironmentVariables()
                .AddJsonFile(configurationFile, optional: true)
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

        public string GetParameter(ParameterType parameterType)
        {
            return this.GetParameter(parameterType.ToString());
        }


        public int GetParameterInt(ParameterType parameterType, int defaultValue = default)
        {
            int result;
            string param = this.GetParameter(parameterType.ToString());
            if (int.TryParse(param, out result))
            {
                return result;
            }            
            Console.Error.WriteLine($"Parameters Input string {param} is not a valid integer.");
            return defaultValue;            
        }

    }
}
