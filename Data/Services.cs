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

    public async Task<IEnumerable<InvoiceDetailDto>> GetInvoiceDetails(Guid InvoiceId)
    {
      try
      {
        var entities = await Context.InvoiceDetailSet
        .Where(w => w.InvoiceId == InvoiceId && w.RowStatus)
        .ToListAsync();
        var result = entities.Select(To);
        return result;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<IEnumerable<InvoiceDto>> GetInvoices()
    {
      try
      {
        var entities = await Context.InvoiceSet
        .Where(w => w.RowStatus)
        .ToListAsync();
        var result = entities.Select(To);
        return result;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<IEnumerable<RateContractDto>> GetRateContracts()
    {
      try
      {
        var entities = await Context.RateContractSet
        .Where(w => w.RowStatus)
        .ToListAsync();
        var result = entities.Select(To);
        return result;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<IEnumerable<RatePlatformDto>> GetRatePlatforms()
    {
      try
      {
        var entities = await Context.RatePlateformFeeSet
        .Where(w => w.RowStatus)
        .ToListAsync();
        var result = entities.Select(To);
        return result;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactions()
    {
      try
      {
        var entities = await Context.TransactionSet
        .Where(w => w.RowStatus)
        .ToListAsync();
        var result = entities.Select(To);
        return result;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<IEnumerable<TransactionTypeDto>> GetTransactionTypes()
    {
      try
      {
        var entities = await Context.TransactionTypeSet
        .Where(w => w.RowStatus)
        .ToListAsync();
        var result = entities.Select(To);
        return result;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<bool> PutInvoice(InvoiceDto dto)
    {
      try
      {
        // if (await ValidateCompany(dto.CompanyId))
        // throw new Exception($"field name {nameof(dto.CompanyId)} is not FOREIGN KEY in table Companies");
        var result = 0;
        if (dto.Id.HasValue)
        {
          var entity = await Context.InvoiceSet
            .Where(w => w.Id == dto.Id && w.RowStatus)
            .SingleOrDefaultAsync();
          if (entity != null)
          {
            entity.Changes(dto);
            entity.ModifiedBy = "system";
            entity.ModifiedDate = DateTime.Now;
          }
        }
        else
        {
          var entity = new InvoicePlatformFee();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = dto.InvoiceDate;
          await Context.AddAsync(entity);
        }
        result = await Context.SaveChangesAsync();
        return result > 0;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<bool> PutInvoiceDetail(InvoiceDetailDto dto)
    {
      try
      {
        if (await ValidateInvoice(dto.InvoiceId))
        throw new Exception($"field name {nameof(dto.InvoiceId)} is not FOREIGN KEY in table InvoicePlatformFee");

        var result = 0;
        if (dto.Id.HasValue)
        {
          var entity = await Context.InvoiceDetailSet
            .Where(w => w.Id == dto.Id && w.RowStatus)
            .SingleOrDefaultAsync();
          if (entity != null)
          {
            entity.Changes(dto);
            entity.ModifiedBy = "system";
            entity.ModifiedDate = DateTime.Now;
          }
        }
        else
        {
          var entity = new InvoiceDetailPlatformFee();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = DateTime.Now;
          await Context.AddAsync(entity);
        }
        result = await Context.SaveChangesAsync();
        return result > 0;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<bool> PutRateContract(RateContractDto dto)
    {
      try
      {
        var result = 0;
        if (dto.Id.HasValue)
        {
          var entity = await Context.RateContractSet
            .Where(w => w.Id == dto.Id && w.RowStatus)
            .SingleOrDefaultAsync();
          if (entity != null)
          {
            entity.Changes(dto);
            entity.ModifiedBy = "system";
            entity.ModifiedDate = DateTime.Now;
          }
        }
        else
        {
          var entity = new RateContract();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = DateTime.Now;
          await Context.AddAsync(entity);
        }
        result = await Context.SaveChangesAsync();
        return result > 0;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<bool> PutRatePlatform(RatePlatformDto dto)
    {
      try
      {
        var result = 0;
        if (dto.Id.HasValue)
        {
          var entity = await Context.RatePlateformFeeSet
            .Where(w => w.Id == dto.Id && w.RowStatus)
            .SingleOrDefaultAsync();
          if (entity != null)
          {
            entity.Changes(dto);
            entity.ModifiedBy = "system";
            entity.ModifiedDate = DateTime.Now;
          }
        }
        else
        {
          var entity = new RatePlateformFee();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = DateTime.Now;
          await Context.AddAsync(entity);
        }
        result = await Context.SaveChangesAsync();
        return result > 0;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<bool> PutTransaction(TransactionDto dto)
    {
      try
      {
        var result = 0;
        if (dto.Id.HasValue)
        {
          var entity = await Context.TransactionSet
            .Where(w => w.Id == dto.Id && w.RowStatus)
            .SingleOrDefaultAsync();
          if (entity != null)
          {
            entity.Changes(dto);
            entity.ModifiedBy = "system";
            entity.ModifiedDate = DateTime.Now;
          }
        }
        else
        {
          var entity = new Transaction();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = DateTime.Now;
          await Context.AddAsync(entity);
        }
        result = await Context.SaveChangesAsync();
        return result > 0;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<bool> PutTransactionType(TransactionTypeDto dto)
    {
      try
      {
        var result = 0;
        if (dto.Id.HasValue)
        {
          var entity = await Context.TransactionTypeSet
            .Where(w => w.Id == dto.Id && w.RowStatus)
            .SingleOrDefaultAsync();
          if (entity != null)
          {
            entity.Changes(dto);
            entity.ModifiedBy = "system";
            entity.ModifiedDate = DateTime.Now;
          }
        }
        else
        {
          var entity = new TransactionType();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = DateTime.Now;
          await Context.AddAsync(entity);
        }
        result = await Context.SaveChangesAsync();
        return result > 0;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }
    
    private async Task<bool> ValidateCompany(Guid CompanyId)
    {
      try
      {
        var entity = await Context.Set<Company>()
        .Where(w => w.Id == CompanyId && w.RowStatus)
        .SingleOrDefaultAsync();
        return entity != null;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    private async Task<bool> ValidateInvoice(Guid InvoiceId)
    {
      try
      {
        var entity = await Context.InvoiceSet
        .Where(w => w.Id == InvoiceId && w.RowStatus)
        .SingleOrDefaultAsync();
        return entity != null;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }
  }

  public interface IService
  {
    Task<IEnumerable<InvoiceDto>> GetInvoices();
    Task<IEnumerable<InvoiceDetailDto>> GetInvoiceDetails(Guid InvoiceId);
    Task<IEnumerable<RateContractDto>> GetRateContracts();
    Task<IEnumerable<RatePlatformDto>> GetRatePlatforms();
    Task<IEnumerable<TransactionDto>> GetTransactions();
    Task<IEnumerable<TransactionTypeDto>> GetTransactionTypes();
    Task<bool> PutInvoice(InvoiceDto dto);
    Task<bool> PutInvoiceDetail(InvoiceDetailDto dto);
    Task<bool> PutRateContract(RateContractDto dto);
    Task<bool> PutRatePlatform(RatePlatformDto dto);
    Task<bool> PutTransaction(TransactionDto dto);
    Task<bool> PutTransactionType(TransactionTypeDto dto);
  }
}