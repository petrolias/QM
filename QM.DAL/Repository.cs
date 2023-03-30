using QM.DAL.Abstractions;
using QM.Models.Abstractions;

namespace QM.DAL
{
    public class Repository : IRepository
    {
        private readonly QMDBContext _dbContext;
        public Repository(QMDBContext context)
        {
            this._dbContext = context;
        }

        public async Task SaveChangesAsync<TRegistrationModel>(TRegistrationModel registrationModel)
                where TRegistrationModel : IRegistrationModel
        {
            try
            {
                await _dbContext.AddAsync(registrationModel);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error persisting registration: {ex.Message}");
                throw ex;
            }

        }
    }
}
