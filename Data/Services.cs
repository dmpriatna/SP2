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

    public async Task<IEnumerable<ContractDto>> GetContracts()
    {
      try
      {
        var entities = await Context.ContractSet
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

    public async Task<IEnumerable<RatePlatformList>> GetRatePlatforms()
    {
      try
      {
        var entities = await Context.RatePlateformFeeSet
        .Where(w => w.RowStatus)
        .Include(i => i.TrxType)
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

    public async Task<Guid> PutInvoice(InvoiceDto dto)
    {
      try
      {
        if (await ValidateCompany(dto.CompanyId))
        throw new Exception($"field name {nameof(dto.CompanyId)} is not FOREIGN KEY in table Companies");
        Guid result;
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
          result = entity.Id;
        }
        else
        {
          var entity = new InvoicePlatformFee();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = dto.InvoiceDate;
          entity.RowStatus = true;
          await Context.AddAsync(entity);
          result = entity.Id;
        }
        await Context.SaveChangesAsync();
        return result;
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
          entity.RowStatus = true;
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
          entity.RowStatus = true;
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
          entity.RowStatus = true;
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
            await Contract(entity);
          }
        }
        else
        {
          var entity = new Transaction();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = DateTime.Now;
          entity.RowStatus = true;
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

    private async Task Contract(Transaction t)
    {
      try
      {
        Contract contract;
        RateContract rateContract = null;
        RatePlateformFee ratePlateform;

        contract = await Context.ContractSet
        .Where(w => w.CompanyId == t.CompanyId)
        .FirstOrDefaultAsync();

        if (contract != null)
        rateContract = await Context.RateContractSet
        .Where(w => w.ContractId == contract.Id)
        .SingleOrDefaultAsync();
        
        ratePlateform = await Context.RatePlateformFeeSet
        .Where(w => w.TransactionTypeId == t.TransactionTypeId)
        .SingleOrDefaultAsync();

        var total = rateContract?.RateNominal + ratePlateform?.RateNominal;

        var invoiceId = await PutInvoice(new InvoiceDto
        {
          CompanyId = t.CompanyId,
          InvoiceDate = DateTime.Now,
          InvoiceNumber = "INV" + DateTime.Now.ToString("yyyyMMddHHmmss"),
          IsContract = contract != null,
          JobNumber = t.JobNumber,
          PaidThru = DateTime.Now,
          TotalAmount = total ?? 0
        });

        if (rateContract != null)
        await PutInvoiceDetail(new InvoiceDetailDto
        {
          InvoiceAmount = rateContract.RateNominal,
          InvoiceId = invoiceId,
          TransactionTypeId = rateContract.TransactionTypeId
        });

        await PutInvoiceDetail(new InvoiceDetailDto
        {
          InvoiceAmount = ratePlateform.RateNominal,
          InvoiceId = invoiceId,
          TransactionTypeId = ratePlateform.TransactionTypeId
        });
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
          entity.RowStatus = true;
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
        string jobNumber = null;
        if (dto.Id.HasValue)
        {
          jobNumber = await Context.JobNumber();
          var entity = await Context.SP2
            .Where(w => w.Id == dto.Id)
            .SingleOrDefaultAsync();
          if (entity != null)
          {
            entity.Changes(dto);
            entity.JobNumber = jobNumber;
            entity.ModifiedBy = "system";
            entity.ModifiedDate = DateTime.Now;
            entity.PositionStatus = dto.IsDraft ? 0 : 2;
            entity.RowStatus = dto.RowStatus ?? true;

            await SP2Container(dto.Containers, entity.Id);
            await SP2Notify(dto.Notifies, entity.Id);

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

            await PutLog(new LogDto
            {
              PositionStatus = entity.PositionStatus
            }, entity.Id);
          }
        }
        else
        {
          jobNumber = dto.JobNumber;
          var entity = new SuratPenyerahanPetikemas();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = DateTime.Now;
          entity.PositionStatus = dto.IsDraft ? 0 : 2;
          entity.RowStatus = true;
          await Context.AddAsync(entity);

          await SP2Container(dto.Containers, entity.Id);
          await SP2Notify(dto.Notifies, entity.Id);

          await PutLog(new LogDto
          {
            PositionStatus = entity.PositionStatus
          }, entity.Id);
        }
        await Context.SaveChangesAsync();
        return jobNumber;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    private async Task SP2Container(ContainerDto[] collection, Guid SP2Id)
    {
      if (collection == null) return;
      foreach (var item in collection)
      {
        await PutContainer(item, SP2Id);
      }
    }

    private async Task PutContainer(ContainerDto dto, Guid SP2Id)
    {
      try
      {
        if (dto.Id.HasValue)
        {
          var entity = await Context.SP2Container
            .Where(w => w.Id == dto.Id && w.SuratPenyerahanPetikemasId == SP2Id)
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
          var entity = new Container();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = DateTime.Now;
          entity.SuratPenyerahanPetikemasId = SP2Id;
          entity.RowStatus = true;
          await Context.AddAsync(entity);
        }
        await Context.SaveChangesAsync();
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    private async Task SP2Notify(NotifyDto[] collection, Guid SP2Id)
    {
      if (collection == null) return;
      foreach (var item in collection)
      {
        await PutNotify(item, SP2Id);
      }
    }

    private async Task PutNotify(NotifyDto dto, Guid SP2Id)
    {
      try
      {
        if (dto.Id.HasValue)
        {
          var entity = await Context.SP2Notify
            .Where(w => w.Id == dto.Id && w.SuratPenyerahanPetikemasId == SP2Id)
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
          var entity = new Notify();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = DateTime.Now;
          entity.SuratPenyerahanPetikemasId = SP2Id;
          entity.RowStatus = true;
          await Context.AddAsync(entity);
        }
        await Context.SaveChangesAsync();
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    private async Task PutLog(LogDto dto, Guid SP2Id)
    {
      try
      {
        if (dto.Id.HasValue)
        {
          var entity = await Context.SP2Log
            .Where(w => w.Id == dto.Id && w.SuratPenyerahanPetikemasId == SP2Id)
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
          var entity = new Log();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = DateTime.Now;
          entity.SuratPenyerahanPetikemasId = SP2Id;
          entity.RowStatus = true;
          await Context.AddAsync(entity);
        }
        await Context.SaveChangesAsync();
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
        var entity = await Context.CompanySet
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

    private async Task<bool> ValidateSP2(Guid SP2Id)
    {
      try
      {
        var entity = await Context.SP2
        .Where(w => w.Id == SP2Id && w.RowStatus)
        .SingleOrDefaultAsync();
        return entity == null;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<Tuple<IEnumerable<SP2List>, int>> ListSP2(ListSP2Request request)
    {
      try
      {
        var entities = new List<SuratPenyerahanPetikemas>();
        var orders = string.Join(',',
            request.Orders.Where(w => !string.IsNullOrWhiteSpace(w)));
        var query = Context.SP2.Where(w => w.RowStatus);

        if (request.Status == 0)
        {
          query = query.Where(w => w.PositionStatus > 0 && w.PositionStatus < 4);
        }
        else if (request.Status == 1)
        {
          query = query.Where(w => w.PositionStatus == 0);
        }
        else
        {
          query = query.Where(w => w.PositionStatus == 4);
        }

        if (!string.IsNullOrWhiteSpace(request.PaymentMethod))
        {
          query = query
            .Where(w => w.PaymentMethod.ToLower() == request.PaymentMethod.ToLower());
        }

        var countAll = query.Count();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
          var all = request.Search.ToLower();
          query = query.Where(w => w.JobNumber.ToLower().Contains(all) ||
            w.TypeTransaction.ToLower().Contains(all) ||
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
        
        return Tuple.Create<IEnumerable<SP2List>, int>(entities.Select(ToList), countAll);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public async Task<SP2Detail> DetailSP2(Guid Id)
    {
      try
      {
        var result = await Context.SP2
        .Where(w => w.RowStatus && w.Id == Id)
        .Include(i => i.Containers)
        .Include(i => i.Logs)
        .Include(i => i.Notifies)
        .SingleOrDefaultAsync();
        return To(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<int> UpdateStatus(SP2StatusRequest request)
    {
      try
      {
        int result = -1;
        var set = Context.SP2.AsQueryable();
        if (request.Id.HasValue)
        set = set.Where(w => w.Id == request.Id.Value);
        if (!string.IsNullOrWhiteSpace(request.JobNumber))
        set = set.Where(w => w.JobNumber == request.JobNumber);

        var entity = await set.SingleOrDefaultAsync();
        if (entity == null) return result;
        result = entity.PositionStatus;
        
        await PutLog(new LogDto
        {
          PositionStatus = (int)request.Status
        }, entity.Id);

        entity.PositionStatus = (int)request.Status;
        await Context.SaveChangesAsync();
        return result;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<Guid> PutContract(ContractDto dto)
    {
      try
      {
        if (await ValidateCompany(dto.CompanyId))
        throw new Exception($"field name {nameof(dto.CompanyId)} is not FOREIGN KEY in table Companies");
        Guid result;
        if (dto.Id.HasValue)
        {
          var entity = await Context.ContractSet
            .Where(w => w.Id == dto.Id && w.RowStatus)
            .SingleOrDefaultAsync();
          if (entity != null)
          {
            entity.Changes(dto);
            entity.ModifiedBy = "system";
            entity.ModifiedDate = DateTime.Now;
          }
          result = entity.Id;
        }
        else
        {
          var entity = new InvoicePlatformFee();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = DateTime.Now;
          entity.RowStatus = true;
          await Context.AddAsync(entity);
          result = entity.Id;
        }
        await Context.SaveChangesAsync();
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
    Task<SP2Detail> DetailSP2(Guid Id);
    Task<IEnumerable<ContractDto>> GetContracts();
    Task<IEnumerable<InvoiceDto>> GetInvoices();
    Task<IEnumerable<InvoiceDetailDto>> GetInvoiceDetails(Guid InvoiceId);
    Task<IEnumerable<RateContractDto>> GetRateContracts();
    Task<IEnumerable<RatePlatformList>> GetRatePlatforms();
    Task<IEnumerable<TransactionDto>> GetTransactions();
    Task<IEnumerable<TransactionTypeDto>> GetTransactionTypes();
    Task<Tuple<IEnumerable<SP2List>, int>> ListSP2(ListSP2Request request);
    Task<Guid> PutContract(ContractDto dto);
    Task<Guid> PutInvoice(InvoiceDto dto);
    Task<bool> PutInvoiceDetail(InvoiceDetailDto dto);
    Task<bool> PutRateContract(RateContractDto dto);
    Task<bool> PutRatePlatform(RatePlatformDto dto);
    Task<string> PutSP2(SP2Dto dto);
    Task<bool> PutTransaction(TransactionDto dto);
    Task<bool> PutTransactionType(TransactionTypeDto dto);
    Task SendMail(EmailDto oContent);
    Task<int> UpdateStatus(SP2StatusRequest request);
  }
}