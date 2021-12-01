using Microsoft.EntityFrameworkCore;

namespace SP2.Data
{
    public class GoLogContext : DbContext
    {
        public GoLogContext(DbContextOptions<GoLogContext> options) : base(options)
        {
        }

        public DbSet<Koja> Koja { get; set; }
        public DbSet<SuratPenyerahanPetikemas> SP2 { get; set; }
        public DbSet<Container> SP2Container { get; set; }
        public DbSet<Log> SP2Log { get; set; }

        public DbSet<Company> CompanySet { get; set; }
        public DbSet<Document> DocumentSet { get; set; }

        public DbSet<Transaction> TransactionSet { get; set; }
        public DbSet<TransactionType> TransactionTypeSet { get; set; }
        public DbSet<RatePlateformFee> RatePlateformFeeSet { get; set; }
        public DbSet<RateContract> RateContractSet { get; set; }
        public DbSet<InvoicePlatformFee> InvoiceSet { get; set; }
        public DbSet<InvoiceDetailPlatformFee> InvoiceDetailSet { get; set; }
    }
}