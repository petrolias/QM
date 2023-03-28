using Microsoft.EntityFrameworkCore;
using QM.Core.Abstractions;
using QM.DAL;
using QM.Models.Abstractions;

namespace QM.Core.DB
{
    public class DbStorage : IStorage
    {
        private readonly QMDBContext _dbContext;   

        public DbStorage(QMDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
       
        public async Task SaveAsync<TRegistrationModel>(TRegistrationModel registrationModel)
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
            }

        }
    }
}
