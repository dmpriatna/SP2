using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SP2.Data
{
  public class Services : Mapping, IService
  {
    public Services(GoLogContext _context)
    {
      Context = _context;
    }

    private GoLogContext Context { get; }

    public async Task<IEnumerable<InvoiceDetailDto>> InvoiceDetail(Guid InvoiceId)
    {
      try
      {
        var entities = await Context
        .Set<InvoiceDetailPlatformFee>()
        .Where(w => w.InvoiceId == InvoiceId)
        .ToListAsync();
        var result = entities.Select(To);
        return result;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<IEnumerable<InvoiceDto>> Invoices()
    {
      try
      {
        var entities = await Context
        .Set<InvoicePlatformFee>()
        .ToListAsync();
        var result = entities.Select(To);
        return result;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<IEnumerable<TransactionDto>> Transactions()
    {
      try
      {
        var entities = await Context
        .Set<Transaction>()
        .ToListAsync();
        var result = entities.Select(To);
        return result;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }
  }

  public interface IService
  {
    Task<IEnumerable<InvoiceDto>> Invoices();
    Task<IEnumerable<InvoiceDetailDto>> InvoiceDetail(Guid InvoiceId);
    Task<IEnumerable<TransactionDto>> Transactions();
  }
}