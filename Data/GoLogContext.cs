using Microsoft.EntityFrameworkCore;

namespace SP2.Data
{
    public class GoLogContext : DbContext
    {
        public GoLogContext(DbContextOptions<GoLogContext> options) : base(options)
        {
        }

        public DbSet<DOContainer> Containers { get; set; }
        public DbSet<Koja> Koja { get; set; }
        public DbSet<SuratPenyerahanPetikemas> SP2 { get; set; }
    }
}