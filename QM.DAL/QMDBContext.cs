using Microsoft.EntityFrameworkCore;
using QM.DAL.Models;

namespace QM.DAL
{
    public class QMDBContext : DbContext
    {
        public DbSet<RegistrationModelDB> Registrations { get; set; }
    }
}
