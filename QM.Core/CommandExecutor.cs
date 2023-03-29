using QM.Core.Abstractions.Enums;

namespace QM.Core
{
    public class CommandExecutor
    {
        private List<PersistSystemType> toExecuteList;
        private List<(PersistSystemType, bool)> executedList;
        private DateTime executionDateTime;
        private string message;

        public CommandExecutor(List<PersistSystemType> toExecuteList, string message)
        {
            this.toExecuteList = toExecuteList;
            this.executedList = new List<(PersistSystemType, bool)>();
            this.executionDateTime = DateTime.UtcNow;
            this.message = message;
        }

        public async Task ExecuteCommandsAsync()
        {
            while (toExecuteList.Count > 0)
            {
                var tasks = new List<Task>();
                foreach (var command in toExecuteList)
                {
                    tasks.Add(ExecuteCommandAsync(command));
                }
                await Task.WhenAll(tasks);

                foreach (var (command, success) in executedList)
                {
                    toExecuteList.Remove(command);
                }
                executedList.Clear();
            }
        }

        private async Task ExecuteCommandAsync(PersistSystemType persistSystemType)
        {
            try
            {
                // Execute the command here and record the execution time
                DateTime startTime = DateTime.UtcNow;
                Console.WriteLine($"Executing {persistSystemType}...");
                await Task.Delay(1000); // Simulate command execution time
                DateTime endTime = DateTime.UtcNow;
                Console.WriteLine($"Executed {persistSystemType} successfully in {(endTime - startTime).TotalMilliseconds} ms.");
                executedList.Add((persistSystemType, true));
            }
            catch (Exception e)
            {
                // Record the failure time and log the exception message
                Console.WriteLine($"Failed to execute {persistSystemType}: {e.Message}");
                executedList.Add((persistSystemType, false));
            }
        }
    }
}
