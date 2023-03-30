using QM.Core.Abstractions.Enums;

namespace QM.Core.Abstractions
{
    /// <summary>
    /// Adding multiple strategies here
    /// </summary>
    public static class ExecutionStrategy
    {
        /// <summary>
        /// Define a default strategy to persist data
        /// </summary>
        public static List<PersistStrategyType> DefaultPersistStrategyTypesStrategies = new() {
                PersistStrategyType.File,
                PersistStrategyType.Db
            };

    }
}
