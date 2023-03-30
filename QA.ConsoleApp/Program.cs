namespace QA.ConsoleApp
{
    internal class Program
    {             
        private static Playground<Program> _playground = new();
        static async Task Main(string[] args)
        {
            await _playground.ExecuteTaskAsync();            
        }
      
    }
}