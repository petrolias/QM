using QM.Core.Abstractions.Enums;
using QM.Models.Abstractions;

namespace QM.Core.Abstractions
{
    public interface IConsumerPostNotifyPublisher<TAppContext, TInputModel> where TInputModel : IRegistrationModel
    {
        Task ExecutePostNotifications(
           TInputModel inputModel,
           DateTime perstistedDateTimeAt,
           DateTime persistedDateTimeEnd,
           List<PersistStrategyType> persistStrategyTypes
           );
    }
        
}