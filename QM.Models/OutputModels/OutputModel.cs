using QM.Core.Abstractions.Enums;

namespace QM.Models.OutputModels
{
    public class OutputModel<TModel>
    {
        public TModel Model { get; set; }        

        public DateTime PersistedDateTimeAt { get; }
        public DateTime PersistedDateTimeEnd { get; }

        public TimeSpan TotalPesistTime { get; }

        public List<PersistStrategyType> PersistStrategyTypes = new();

        public OutputModel(TModel model,             
            DateTime persistedDateTimeAt, 
            DateTime persistedDateTimeEnd, 
            List<PersistStrategyType> persistStrategyTypes)
        {
            this.Model = model;
            this.PersistedDateTimeAt = persistedDateTimeAt;
            this.PersistedDateTimeEnd = persistedDateTimeEnd;
            this.PersistStrategyTypes = persistStrategyTypes;
            this.TotalPesistTime = this.GetTotalPesistTime();
        }      

        private TimeSpan GetTotalPesistTime()
        {
            return this.PersistedDateTimeEnd - this.PersistedDateTimeAt;
        }

    }
}

