namespace QM.Models.InputModels
{
    internal class BaseRegistration
    {
        public BaseRegistration()
        {
            this.CreatedAt = DateTime.UtcNow;
        }

        public BaseRegistration(DateTime createdAt)
        {
            this.CreatedAt = createdAt;
        }

        public DateTime CreatedAt { get; set; }        
    }
}
