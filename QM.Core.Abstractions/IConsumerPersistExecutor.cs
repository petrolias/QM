using QM.Core.Abstractions.Enums;
using QM.Models.Abstractions;

namespace QM.Core.Abstractions
{
    public interface IConsumerPersistExecutor<TAppContext, TInputModel>  where TInputModel : IRegistrationModel
    {
        Task ConsumeAndPersistAsync(
            List<PersistStrategyType> toExecuteList,
            TInputModel inputModel);
    }
}