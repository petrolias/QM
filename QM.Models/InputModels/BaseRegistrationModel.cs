using QM.Models.Abstractions;

namespace QM.Models.InputModels
{
    public class BaseRegistrationModel : IBaseRegistrationModel
    {
        public BaseRegistrationModel()
        {
            this.CreatedAt = DateTime.UtcNow;
        }

        public BaseRegistrationModel(DateTime createdAt)
        {
            this.CreatedAt = createdAt;
        }

        public DateTime CreatedAt { get; set; }
    }
}
