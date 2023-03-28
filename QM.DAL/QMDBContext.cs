using Microsoft.EntityFrameworkCore;
using QM.DAL.Models;

namespace QM.DAL
{
    public class QMDBContext : DbContext
    {
        public QMDBContext(DbContextOptions<QMDBContext> options) : base(options)
        {
        }

        public DbSet<RegistrationModelDB> Registrations { get; set; }
    }
}
