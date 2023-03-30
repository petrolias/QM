using QM.Core.Abstractions.Enums;

namespace QM.Core.Abstractions
{
    /// <summary>
    /// Adding multiple strategies here
    /// </summary>
    public static class ExecutionStrategy
    {
        public static List<PersistStrategyType> DefaultPersistStrategyTypesStragegy = new() {
                PersistStrategyType.File,
                PersistStrategyType.Db
            };

    }
}
