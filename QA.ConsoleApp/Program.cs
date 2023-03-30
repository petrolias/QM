using QA.External.Models;

namespace QA.ConsoleApp
{
    internal class Program
    {             
        private static Playground<Program> _playground = new();
        static async Task Main(string[] args)
        {
            var inputRegistrationModel = new InputRegistrationModel() { UserId = 1, UserName = "TestUsername" };
            await _playground.ExecuteTaskAsync(inputRegistrationModel);            
        }
      
    }
}