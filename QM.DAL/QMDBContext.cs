using Microsoft.EntityFrameworkCore;
using QM.Mapper.Models;

namespace QM.Mapper
{
    public class QMDBContext : DbContext
    {
        public QMDBContext(DbContextOptions<QMDBContext> options) : base(options)
        {
        }

        public DbSet<RegistrationModelDB> Registrations { get; set; }
    }
}
