using QM.Core.Abstractions.Enums;

namespace QM.Core.Abstractions
{
    /// <summary>
    /// Adding multiple strategies here
    /// </summary>
    public static class ExecutionStrategy
    {
        public static List<PersistSystemType> DefaultPersistSystemTypesStragegy = new() {
                PersistSystemType.File,
                PersistSystemType.Db
            };

    }
}
