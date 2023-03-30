using QM.Models.Abstractions;

namespace QM.Models.DomainModels
{
    public class RegistrationModel : BaseRegistrationModel, IRegistrationModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}