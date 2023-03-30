using QM.Models.Abstractions;

namespace QM.Models.DomainModels
{
    public class BaseRegistrationModel : IBaseRegistrationModel
    {
        public BaseRegistrationModel()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.Guid = Guid.NewGuid();
        }

        public BaseRegistrationModel(DateTime createdAt)
        {
            this.CreatedAt = createdAt;
        }

        public DateTime CreatedAt { get; set; }
        public Guid Guid { get; set; }
    }
}
