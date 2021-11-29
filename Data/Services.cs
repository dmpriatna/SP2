using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SP2.Data
{
  public class Services : Mapping, IService
  {
    public Services(GoLogContext _context,
      IConfiguration config)
    {
      Context = _context;
      EmailService = config["GologService"] + "/api/Email/GatePass";
    }

    private GoLogContext Context { get; }
    private string EmailService { get; }

    public async Task SendMail(EmailDto oContent)
    {
      try
      {
        using (var client = new HttpClient())
        {
          var sContent = Newtonsoft.Json.JsonConvert.SerializeObject(oContent);
          var content = new StringContent(sContent, System.Text.Encoding.UTF8, "application/json");
          await client.PostAsync(EmailService, content);
        }
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

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

    public async Task<IEnumerable<RatePlatformWithRelationDto>> GetRatePlatforms()
    {
      try
      {
        var entities = await Context.RatePlateformFeeSet
        .Where(w => w.RowStatus)
        .Include(i => i.TrxType)
        .ToListAsync();
        var result = entities.Select(TWR);
        return result;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    private RatePlatformWithRelationDto TWR(RatePlateformFee entity)
    {
      return new RatePlatformWithRelationDto
      {
        Id = entity.Id,
        RateNominal = entity.RateNominal,
        RowStatus = entity.RowStatus,
        TransactionAlias = entity.TrxType.TransactionAlias,
        TransactionTypeId = entity.TransactionTypeId
      };
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
        if (await ValidateCompany(dto.CompanyId))
        throw new Exception($"field name {nameof(dto.CompanyId)} is not FOREIGN KEY in table Companies");
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
        if (await ValidateTransactionType(dto.TransactionTypeId))
        throw new Exception($"field name {nameof(dto.TransactionTypeId)} is not FOREIGN KEY in table TransactionType");

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
        if (await ValidateTransactionType(dto.TransactionTypeId))
        throw new Exception($"field name {nameof(dto.TransactionTypeId)} is not FOREIGN KEY in table TransactionType");

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
        if (await ValidateCompany(dto.CompanyId))
        throw new Exception($"field name {nameof(dto.CompanyId)} is not FOREIGN KEY in table Companies");
        if (await ValidateTransactionType(dto.TransactionTypeId))
        throw new Exception($"field name {nameof(dto.TransactionTypeId)} is not FOREIGN KEY in table TransactionType");

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

    public async Task<string> PutSP2(SP2Dto dto)
    {
      try
      {
        var result = 0;
        SuratPenyerahanPetikemas entity = null;
        if (dto.Id.HasValue)
        {
          entity = await Context.SP2
            .Where(w => w.Id == dto.Id)
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
          var jobNumber = await Context.JobNumber();
          entity = new SuratPenyerahanPetikemas();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = DateTime.Now;
          entity.JobNumber = jobNumber;
          await Context.AddAsync(entity);

          var trxNumber = await Context.TrxNumber();
          await PutTransaction(new TransactionDto
          {
            CompanyId = Guid.Parse("831ac973-af04-4406-8a90-c06dd025989d"),
            Delegated = true,
            JobNumber = entity.JobNumber,
            RowStatus = true,
            TransactionNumber = trxNumber,
            TransactionTypeId = Guid.Parse("8529299c-0a69-494e-ba06-45f844e2a2d0"),
          });
        }
        result = await Context.SaveChangesAsync();
        return entity.JobNumber;
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
        return entity == null;
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
        return entity == null;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    private async Task<bool> ValidateTransactionType(Guid TransactionTypeId)
    {
      try
      {
        var entity = await Context.TransactionTypeSet
        .Where(w => w.Id == TransactionTypeId && w.RowStatus)
        .SingleOrDefaultAsync();
        return entity == null;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<Tuple<IEnumerable<SP2Dto>, int>> ListSP2(ListSP2Request request)
    {
      try
      {
        var query = Context.SP2.Where(w => w.RowStatus);
        var entities = new List<SuratPenyerahanPetikemas>();
        var orders = string.Join(',',
          request.Orders.Where(w => !string.IsNullOrWhiteSpace(w)));

        if (request.IsDraft.HasValue)
        {
          var _isDraft = request.IsDraft.Value;
          query = query.Where(w => w.IsDraft == _isDraft);
        }

        var countAll = query.Count();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
          var all = request.Search .ToLower();
          query = query.Where(w => w.BLNumber.ToLower().Contains(all) ||
            w.DocumentName.ToLower().Contains(all) ||
            w.DONumber.ToLower().Contains(all) ||
            w.JobNumber.ToLower().Contains(all) ||
            w.PIBNumber.ToLower().Contains(all) ||
            w.SPPBNumber.ToLower().Contains(all) ||
            w.TerminalName.ToLower().Contains(all) ||
            w.TransactionName.ToLower().Contains(all));
        }

        if (!string.IsNullOrWhiteSpace(orders))
        {
          query = query.OrderBy(orders);
        }

        if (request.Start > 0)
        {
          query = query.Skip(request.Start);
        }

        if (request.Length > 0)
        {
          query = query.Take(request.Length);
        }

        entities = await query.ToListAsync();
        var result = Tuple.Create<IEnumerable<SP2Dto>, int>(entities.Select(To), countAll);
        return result;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public async Task<SP2Dto> DetailSP2(Guid Id)
    {
      try
      {
        var result = await Context.SP2
        .Where(w => w.RowStatus && w.Id == Id)
        .SingleOrDefaultAsync();
        return To(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }
  }

  public interface IService
  {
    Task<SP2Dto> DetailSP2(Guid Id);
    Task<IEnumerable<InvoiceDto>> GetInvoices();
    Task<IEnumerable<InvoiceDetailDto>> GetInvoiceDetails(Guid InvoiceId);
    Task<IEnumerable<RateContractDto>> GetRateContracts();
    Task<IEnumerable<RatePlatformWithRelationDto>> GetRatePlatforms();
    Task<IEnumerable<TransactionDto>> GetTransactions();
    Task<IEnumerable<TransactionTypeDto>> GetTransactionTypes();
    Task<Tuple<IEnumerable<SP2Dto>, int>> ListSP2(ListSP2Request request);
    Task<bool> PutInvoice(InvoiceDto dto);
    Task<bool> PutInvoiceDetail(InvoiceDetailDto dto);
    Task<bool> PutRateContract(RateContractDto dto);
    Task<bool> PutRatePlatform(RatePlatformDto dto);
    Task<string> PutSP2(SP2Dto dto);
    Task<bool> PutTransaction(TransactionDto dto);
    Task<bool> PutTransactionType(TransactionTypeDto dto);
    Task SendMail(EmailDto oContent);
  }
}