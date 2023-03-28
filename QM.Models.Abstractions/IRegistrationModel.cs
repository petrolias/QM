namespace QM.Models.Abstractions
{
    public interface IRegistrationModel : IBaseRegistrationModel
    {
        int UserId { get; set; }
        string UserName { get; set; }
    }
}