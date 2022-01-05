using System.Linq;
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
    public DbSet<Notify> SP2Notify { get; set; }

    public DbSet<Contract> ContractSet { get; set; }
    public DbSet<Company> CompanySet { get; set; }
    public DbSet<Document> DocumentSet { get; set; }

    public DbSet<Transaction> TransactionSet { get; set; }
    public DbSet<TransactionType> TransactionTypeSet { get; set; }
    public DbSet<RatePlateformFee> RatePlateformFeeSet { get; set; }
    public DbSet<RateContract> RateContractSet { get; set; }
    public DbSet<InvoicePlatformFee> InvoiceSet { get; set; }
    public DbSet<InvoiceDetailPlatformFee> InvoiceDetailSet { get; set; }

    public DbSet<DeliveryOrder> DeliveryOrderSet { get; set; }

    public string InvoiceNumber
    {
      get
      {
        int one = 1;
        string suffix = one.ToString("D6");
        string result = null;

        var now = System.DateTime.Now;
        var patern = System.DateTime.Now.ToString("yyyy-MM/INV/SP2/dd-");

        var lastCode = SP2
          .Where(w => w.CreatedDate.Date == now.Date)
          .OrderBy(ob => ob.CreatedDate)
          .LastOrDefault();

        if (lastCode == null)
          result = patern + suffix;
        else
        {
          if (string.IsNullOrWhiteSpace(lastCode.JobNumber))
            result = patern + suffix;
          else
          {
            var chunk = lastCode.JobNumber.Split('-').Last();
            if (int.TryParse(chunk, out int code))
            {
              code++;
              result = patern + code.ToString("D6");
            }
            else
              result = patern + now.ToString("HHmmss");
          }
        }
        return result;
      }
    }
  }
}