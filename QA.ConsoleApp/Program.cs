using Microsoft.Extensions.DependencyInjection;
using QM.DAL.Abstractions;
using QM.DAL.Models;
using QM.Core.Abstractions.Enums;
using QM.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace QA.ConsoleApp
{
    internal class Program
    {             
        private static Playground _playground = new Playground();
        static async Task Main(string[] args)
        {
            await _playground.TestTaskAsync();

            Console.WriteLine("Hello, World!");
        }
      
    }
}