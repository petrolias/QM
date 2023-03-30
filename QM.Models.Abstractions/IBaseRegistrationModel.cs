namespace QM.Models.Abstractions
{
    public interface IBaseRegistrationModel
    {
        DateTime CreatedAt { get; set; }
        Guid Guid { get; set; }
    }
}