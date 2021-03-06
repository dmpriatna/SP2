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

    public async Task<IEnumerable<ContractDto>> GetContracts(Guid? CompanyId)
    {
      try
      {
        var entities = await Context.ContractSet
        .Where(w => w.RowStatus)
        .ToListAsync();

        if (CompanyId.HasValue)
        entities = entities.Where(w => w.CompanyId == CompanyId).ToList();

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

        Transaction entity;
        if (dto.Id.HasValue)
        {
          entity = await Context.TransactionSet
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
          entity = new Transaction();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedBy = "system";
          entity.CreatedDate = DateTime.Now;
          entity.RowStatus = true;
          await Context.AddAsync(entity);
          await Contract(entity);
        }
        return await Context.SaveChangesAsync() > 0;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<ContractDto> Contract(Guid Id)
    {
      var entity = await Context.ContractSet
      .Where(w => w.Id == Id)
      .SingleOrDefaultAsync();

      return new ContractDto
      {
        BillingPeriod = entity.BillingPeriod,
        CargoOwnerId = entity.CargoOwnerId,
        CompanyId = entity.CompanyId,
        ContractNumber = entity.ContractNumber,
        EmailPPJK = entity.EmailPPJK,
        EndDate = entity.EndDate,
        FirstParty = entity.FirstParty,
        Id = entity.Id,
        PriceRate = entity.PriceRate,
        RowStatus = entity.RowStatus,
        SecondParty = entity.SecondParty,
        Services = entity.Services,
        StartDate = entity.StartDate
      };
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
          InvoiceNumber = Context.InvoiceNumber,
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
        string jobNumber = dto.IsDraft ? null : Context.JobNumberS;
        if (dto.Id.HasValue)
        {
          var entity = await Context.SP2
            .Where(w => w.Id == dto.Id)
            .SingleOrDefaultAsync();
          if (entity != null)
          {
            entity.Changes(dto);
            entity.JobNumber = jobNumber;
            entity.ModifiedBy = entity.CreatedBy;
            entity.ModifiedDate = DateTime.Now;
            entity.PositionStatus = dto.IsDraft ? 0 : 2;
            entity.RowStatus = 1;

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
          var now = DateTime.Now;
          var entity = new SuratPenyerahanPetikemas();
          entity.Changes(dto);
          entity.Id = Guid.NewGuid();
          entity.CreatedDate = now;
          entity.JobNumber = jobNumber;
          entity.ModifiedDate = now;
          entity.PositionStatus = dto.IsDraft ? 0 : 2;
          entity.RowStatus = 1;
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
      var entities = await Context.SP2Notify
        .Where(w => w.SuratPenyerahanPetikemasId == SP2Id)
        .ToListAsync();
      
      if (collection == null)
      {
        foreach (var item in entities)
        {
          Context.SP2Notify.Remove(item);
        }
        await Context.SaveChangesAsync();
        return;
      }
      
      var toAdd = collection
        .Select(s => new { Email = s.Email })
        .Except(entities.Select(s => new { Email = s.Email }))
        .ToList();

      var toRemove = entities
        .Select(s => new { Email = s.Email })
        .Except(collection.Select(s => new { Email = s.Email }))
        .ToList();
      
      foreach (var each in toAdd)
      {
        var entity = new Notify
        {
          CreatedBy = "system",
          CreatedDate = DateTime.Now,
          Email = each.Email,
          Id = Guid.NewGuid(),
          RowStatus = true,
          SuratPenyerahanPetikemasId = SP2Id
        };
        await Context.AddAsync(entity);
      }

      foreach (var each in toRemove)
      {
        var item = entities.Where(w => w.Email == each.Email).SingleOrDefault();
        Context.SP2Notify.Remove(item);
      }

      await Context.SaveChangesAsync();
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
        .Where(w => w.Id == SP2Id && w.RowStatus == 1)
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
        var query = Context.SP2.Where(w => w.RowStatus == 1);

        if (request.Status.HasValue)
        {
          if (request.Status.Value == SP2StatusIn.Delegate)
          {
            query = query.Where(w => w.PositionStatusName.ToLower() == "delegate");
          }
          else
          {
            var IsDelegate = request.IsDelegate;
            var Status = request.Status.Value;
            var active = IsDelegate.HasValue && IsDelegate.Value ? new[] { 1, 2, 3, 4 } : new[] { 1, 2, 3 };
            var complete = IsDelegate.HasValue && IsDelegate.Value ? new[] { 5 } : new[] { 4 };
            var status = Status == SP2StatusIn.Draft ?
              new int[] { 0 } : (Status == SP2StatusIn.Actived ?
              active : complete);

            query = query.Where(w => status.Contains(w.PositionStatus));
          }
        }

        if (request.InProgress.HasValue && request.InProgress.Value)
        {
          var postat0 = new int[] {1,2,3};
          var postat1 = new int[] {2,3,4};
          query = query.Where(w => w.ServiceName == null && postat0.Contains(w.PositionStatus) ||
          w.ServiceName != null && postat1.Contains(w.PositionStatus));
        }

        if (request.IsDelegate.HasValue)
        {
          if (request.IsDelegate.Value)
            query = query.Where(w => w.ServiceName != null);
          else
            query = query.Where(w => w.ServiceName == null);
        }

        if (!string.IsNullOrWhiteSpace(request.CreatedBy))
        {
          query = query.Where(w => w.CreatedBy.ToLower()
            == request.CreatedBy.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(request.FreightForwarderName))
        {
          query = query.Where(w => w.FrieghtForwarderName.ToLower()
            == request.FreightForwarderName.ToLower());
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
        .Where(w => w.RowStatus == 1 && w.Id == Id)
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
        if (dto.CompanyId.HasValue && await ValidateCompany(dto.CompanyId.Value))
        throw new Exception($"field name {nameof(dto.CompanyId)} is not FOREIGN KEY in table Companies");
        if (dto.CargoOwnerId.HasValue && await ValidateCompany(dto.CargoOwnerId.Value))
        throw new Exception($"field name {nameof(dto.CargoOwnerId)} is not FOREIGN KEY in table Companies");
        if (string.IsNullOrWhiteSpace(dto.EmailPPJK))
        throw new Exception($"field name {nameof(dto.EmailPPJK)} cannot be empty");

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
          var entity = new Contract();
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

    public async Task<bool> CancelTransaction(Guid Id, string Reason)
    {
      try
      {
        var sp2 = await Context.SP2
          .Where(w => w.Id == Id && w.RowStatus == 1)
          .SingleOrDefaultAsync();
        if (sp2 != null)
        {
          sp2.CancelReason = Reason;
          sp2.ModifiedBy = "system";
          sp2.ModifiedDate = DateTime.Now;
          sp2.RowStatus = 0;
        }

        var trx = await Context.TransactionSet
          .Where(w => w.JobNumber == sp2.JobNumber && w.RowStatus)
          .SingleOrDefaultAsync();
        if (trx != null)
        {
          trx.CancelReason = Reason;
          trx.ModifiedBy = "system";
          trx.ModifiedDate = DateTime.Now;
          trx.RowStatus = false;
        }

        return await Context.SaveChangesAsync() > 0;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<Tuple<IEnumerable<object>, int>> ListDoSp2(
      int start,
      int lenght,
      string createdBy,
      string forwarderName,
      bool? isDraft,
      bool? isDelegate
    )
    {
      try
      {
        var count = 0;
        var result = new List<object>();

        var doQuery = Context.DeliveryOrderSet
        .Where(w => w.RowStatus == 1);

        if (!string.IsNullOrWhiteSpace(createdBy))
        doQuery = doQuery
        .Where(w => w.CreatedBy.ToLower() == createdBy.ToLower());

        if (!string.IsNullOrWhiteSpace(forwarderName))
        doQuery = doQuery
        .Where(w => w.FrieghtForwarderName.ToLower() == forwarderName.ToLower());

        if (isDraft.HasValue && isDraft.Value)
          doQuery = doQuery.Where(w => w.PositionStatus == 0);

        if (isDraft.HasValue && !isDraft.Value)
          doQuery = doQuery.Where(w => w.PositionStatus > 0);
        
        if (isDelegate.HasValue)
        {
          if (isDelegate.Value)
            doQuery = doQuery.Where(w => w.ServiceName != null);
          else
            doQuery = doQuery.Where(w => w.ServiceName != null);
        }

        var doList = await doQuery
        .ToListAsync();

        result.AddRange(doList.Select(To));

        var sp2Query = Context.SP2
        .Where(w => w.RowStatus == 1);

        if (!string.IsNullOrWhiteSpace(createdBy))
        sp2Query = sp2Query
        .Where(w => w.CreatedBy.ToLower() == createdBy.ToLower());

        if (!string.IsNullOrWhiteSpace(forwarderName))
        sp2Query = sp2Query
        .Where(w => w.FrieghtForwarderName.ToLower() == forwarderName.ToLower());

        if (isDraft.HasValue && isDraft.Value)
          sp2Query = sp2Query.Where(w => w.PositionStatus == 0);

        if (isDraft.HasValue && !isDraft.Value)
          sp2Query = sp2Query.Where(w => w.PositionStatus > 0);

        if (isDelegate.HasValue)
        {
          if (isDelegate.Value)
            sp2Query = sp2Query.Where(w => w.ServiceName != null);
          else
            sp2Query = sp2Query.Where(w => w.ServiceName != null);
        }

        var sp2List = await sp2Query
        .ToListAsync();

        result.AddRange(sp2List.Select(To));

        var ccQuery = Context.CustomClearanceSet
        .Where(w => w.RowStatus > 0);

        if (!string.IsNullOrWhiteSpace(createdBy))
        ccQuery = ccQuery
        .Where(w => w.CreatedBy.ToLower() == createdBy.ToLower());

        // if (!string.IsNullOrWhiteSpace(forwarderName))
        // ccQuery = ccQuery
        // .Where(w => w.FrieghtForwarderName.ToLower() == forwarderName.ToLower());

        if (isDraft.HasValue && isDraft.Value)
          ccQuery = ccQuery.Where(w => w.PositionStatus == 0);

        if (isDraft.HasValue && !isDraft.Value)
          ccQuery = ccQuery.Where(w => w.PositionStatus > 0);
        
        if (isDelegate.HasValue)
        {
          if (isDelegate.Value)
            ccQuery = ccQuery.Where(w => w.RowStatus == 2);
          else
            ccQuery = ccQuery.Where(w => w.RowStatus == 2);
        }

        var ccList = await ccQuery
        .ToListAsync();

        result.AddRange(ccList.Select(To));

        result = result.OrderByDescending(obd => (obd as IDataTransferObject).ModifiedDate).ToList();
        count = result.Count;

        if (start > 0)
        {
          result = result.Skip(start).ToList();
        }

        if (lenght > 0)
        {
          result = result.Take(lenght).ToList();
        }

        return new Tuple<IEnumerable<object>, int>(result, count);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<string> PutTrxDelegate(DelegatePayload payload)
    {
      try
      {
        if (payload.ServiceName == ServiceType.DO)
          return await PutDODelegate(payload);
        else
          return await PutSP2Delegate(payload);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    private async Task<string> PutDODelegate(DelegatePayload payload)
    {
      string Emails = string.Join(";", payload.NotifyEmails);
      string JobNumber = "";

      if (payload.Id.HasValue)
      {
        var entity = await Context.DeliveryOrderSet
        .Where(w => w.Id == payload.Id && w.RowStatus == 1)
        .SingleOrDefaultAsync();
        if (entity != null)
        {
          var ps = entity.PositionStatus;
          var psn = entity.PositionStatusName;

          if (entity.PositionStatus == 0 && !payload.SaveAsDraft)
          {
            JobNumber = Context.JobNumberD;
            entity.JobNumber = JobNumber;
          }
          else
          {
            JobNumber = entity.JobNumber;
          }

          entity.AttorneyLetter = payload.AttorneyLetter;
          entity.BLDocument = payload.BLDocument;
          entity.BillOfLadingNumber = payload.BLNumber;
          entity.ContractNumber = payload.ContractNumber;
          entity.FrieghtForwarderName = payload.FrieghtForwarderName;
          entity.LetterOfIndemnity = payload.LetterOfIndemnity;
          entity.ModifiedBy = payload.CreatedBy;
          entity.ModifiedDate = DateTime.Now;
          entity.NotifyEmails = Emails;
          entity.PositionStatus = payload.PositionStatus;
          entity.PositionStatusName = payload.PositionStatusName;
          entity.SaveAsDraft = payload.SaveAsDraft;
          entity.ServiceName = payload.ServiceName.ToString();

          // if (payload.Containers != null)
          // foreach (var each in payload.Containers)
          // await PutDOContainerDelegate(each, entity.Id);

          if (ps != payload.PositionStatus ||
            psn != payload.PositionStatusName)
          {
            var logEntity = new DLog
            {
              Activity = $"{payload.PositionStatus};{payload.PositionStatusName}",
              BillOfLadingNumber = "",
              CreatedBy = payload.CreatedBy,
              CreatedDate = DateTime.Now,
              Id = Guid.NewGuid(),
              JobNumber = entity.JobNumber,
              ModifiedBy = "",
              ModifiedDate = DateTime.Now,
              RowStatus = 1
            };
            await Context.AddAsync(logEntity);
          }
          await Context.SaveChangesAsync();
        }
      }
      else
      {
        JobNumber = payload.SaveAsDraft ? "" : Context.JobNumberD;
        var newEntity = new DeliveryOrder
        {
          AttorneyLetter = payload.AttorneyLetter,
          BLDocument = payload.BLDocument,
          CreatedBy = payload.CreatedBy,
          CreatedDate = DateTime.Now,
          ContractNumber = payload.ContractNumber,
          FrieghtForwarderName = payload.FrieghtForwarderName,
          Id = Guid.NewGuid(),
          JobNumber = JobNumber,
          LetterOfIndemnity = payload.LetterOfIndemnity,
          ModifiedBy = "",
          ModifiedDate = DateTime.Now,
          NotifyEmails = Emails,
          PositionStatus = payload.PositionStatus,
          PositionStatusName = payload.PositionStatusName,
          RowStatus = 1,
          SaveAsDraft = payload.SaveAsDraft,
          ServiceName = payload.ServiceName.ToString(),

          CustomerCode = "",
          CustomerName = "",
          Consignee = "",
          DeliveryOrderType = "DELEGATE",
          BillOfLadingDate = DateTime.Now,
          BillOfLadingNumber = payload.BLNumber,
          NotifyPartyAdress = "",
          NotifyPartyName = "",
          PortOfDelivery = "",
          PortOfDischarge = "",
          PortOfLoading = "",
          ShippingLineName = "",
          ShippingLineEmail = "",
          Vessel = "",
          VoyageNumber = ""
        };
        await Context.AddAsync(newEntity);
        var logEntity = new DLog
        {
          Activity = $"{payload.PositionStatus};{payload.PositionStatusName}",
          BillOfLadingNumber = "",
          CreatedBy = payload.CreatedBy,
          CreatedDate = DateTime.Now,
          Id = Guid.NewGuid(),
          JobNumber = JobNumber,
          ModifiedBy = "",
          ModifiedDate = DateTime.Now,
          RowStatus = 1
        };
        await Context.AddAsync(logEntity);
        await Context.SaveChangesAsync();
        // if (payload.Containers != null)
        // foreach (var each in payload.Containers)
        // await PutDOContainerDelegate(each, newEntity.Id);
      }
      return JobNumber;
    }

    private async Task PutDOContainerDelegate(DelegateContainerInput input, Guid doId)
    {
      if (input.Id.HasValue)
      {
        var entity = await Context.DOContainerSet
        .Where(w => w.Id == input.Id && w.RowStatus == 1)
        .SingleOrDefaultAsync();
        if (entity != null)
        {
          entity.ContainerNo = input.ContainerNumber;
          entity.ContainerSize = input.ContainerSize;
          entity.ContainerType = input.ContainerType;
          entity.DepoName = input.DepoName;
          entity.GrossWeight = input.GrossWeight;
          entity.LoadType = input.LoadType;
          entity.ModifiedBy = input.CreatedBy;
          entity.ModifiedDate = DateTime.Now;
          entity.PhoneNumber = input.PhoneNumber;
          entity.SealNo = input.SealNumber;
        }
      }
      {
        var newEntity = new DeliveryOrderContainer
        {
          ContainerNo = input.ContainerNumber,
          ContainerSize = input.ContainerSize,
          ContainerType = input.ContainerType,
          CreatedBy = input.CreatedBy,
          CreatedDate = DateTime.Now,
          DeliveryOrderId = doId,
          DepoName = input.DepoName,
          GrossWeight = input.GrossWeight,
          Id = Guid.NewGuid(),
          LoadType = input.LoadType,
          PhoneNumber = input.PhoneNumber,
          RowStatus = 1,
          SealNo = input.SealNumber
        };
        await Context.AddAsync(newEntity);
      }
      await Context.SaveChangesAsync();
    }

    private async Task<string> PutSP2Delegate(DelegatePayload payload)
    {
      string Emails = string.Join(";", payload.NotifyEmails);
      string JobNumber = "";

      if (payload.Id.HasValue)
      {
        var entity = await Context.SP2
        .Where(w => w.Id == payload.Id && w.RowStatus == 1)
        .SingleOrDefaultAsync();
        if (entity != null)
        {
          var ps = entity.PositionStatus;
          var psn = entity.PositionStatusName;

          if (entity.PositionStatus == 0 && !payload.SaveAsDraft)
          {
            JobNumber = Context.JobNumberS;
            entity.JobNumber = JobNumber;
          }
          else
          {
            JobNumber = entity.JobNumber;
          }

          entity.AttorneyLetter = payload.AttorneyLetter;
          entity.BLDocument = payload.BLDocument;
          entity.BLNumber = payload.BLNumber;
          entity.ContractNumber = payload.ContractNumber;
          entity.FrieghtForwarderName = payload.FrieghtForwarderName;
          entity.LetterOfIndemnity = payload.LetterOfIndemnity;
          entity.ModifiedBy = payload.CreatedBy;
          entity.ModifiedDate = DateTime.Now;
          entity.NotifyEmails = Emails;
          entity.PositionStatus = payload.PositionStatus;
          entity.PositionStatusName = payload.PositionStatusName;
          entity.SaveAsDraft = payload.SaveAsDraft;
          entity.ServiceName = payload.ServiceName.ToString();

          // if (payload.Containers != null)
          // foreach (var each in payload.Containers)
          // await PutSP2ContainerDelegate(each, entity.Id);

          if (ps != payload.PositionStatus ||
            psn != payload.PositionStatusName)
          {
            var logEntity = new Log
            {
              CreatedBy = payload.CreatedBy,
              CreatedDate = DateTime.Now,
              Id = Guid.NewGuid(),
              ModifiedBy = "",
              ModifiedDate = DateTime.Now,
              PositionName = payload.PositionStatusName,
              PositionStatus = payload.PositionStatus,
              RowStatus = true,
              SuratPenyerahanPetikemasId = entity.Id
            };
            await Context.AddAsync(logEntity);
          }
        }
      }
      else
      {
        JobNumber = payload.SaveAsDraft ? "" : Context.JobNumberS;
        var newEntity = new SuratPenyerahanPetikemas
        {
          AttorneyLetter = payload.AttorneyLetter,
          BLDocument = payload.BLDocument,
          BLNumber = payload.BLNumber,
          CreatedBy = payload.CreatedBy,
          CreatedDate = DateTime.Now,
          ContractNumber = payload.ContractNumber,
          DueDate = DateTime.Now,
          FrieghtForwarderName = payload.FrieghtForwarderName,
          Id = Guid.NewGuid(),
          JobNumber = JobNumber,
          LetterOfIndemnity = payload.LetterOfIndemnity,
          ModifiedBy = "",
          ModifiedDate = DateTime.Now,
          NotifyEmails = Emails,
          PositionStatus = payload.PositionStatus,
          PositionStatusName = payload.PositionStatusName,
          RowStatus = 1,
          SaveAsDraft = payload.SaveAsDraft,
          ServiceName = payload.ServiceName.ToString()
        };
        await Context.AddAsync(newEntity);
        var logEntity = new Log
        {
          CreatedBy = payload.CreatedBy,
          CreatedDate = DateTime.Now,
          Id = Guid.NewGuid(),
          ModifiedBy = "",
          ModifiedDate = DateTime.Now,
          PositionName = payload.PositionStatusName,
          PositionStatus = payload.PositionStatus,
          RowStatus = true,
          SuratPenyerahanPetikemasId = newEntity.Id
        };
        await Context.AddAsync(logEntity);
        // if (payload.Containers != null)
        // foreach (var each in payload.Containers)
        // await PutSP2ContainerDelegate(each, newEntity.Id);
      }
      await Context.SaveChangesAsync();
      return JobNumber;
    }

    private async Task PutSP2ContainerDelegate(DelegateContainerInput input, Guid sp2Id)
    {
      if (input.Id.HasValue)
      {
        var entity = await Context.SP2Container
        .Where(w => w.Id == input.Id && w.RowStatus)
        .SingleOrDefaultAsync();
        if (entity != null)
        {
          entity.BLNumber = input.BLNumber;
          entity.ContainerNumber = input.ContainerNumber;
          entity.ContainerSize = input.ContainerSize;
          entity.ContainerType = input.ContainerType;
          entity.ModifiedBy = input.CreatedBy;
          entity.ModifiedDate = DateTime.Now;
          entity.VesselName = input.VesselName;
          entity.VesselNumber = input.VesselNumber;
          entity.VoyageNumber = input.VoyageNumber;
        }
      }
      else
      {
        var newEntity = new Container
        {
          BLNumber = input.BLNumber,
          ContainerNumber = input.ContainerNumber,
          ContainerSize = input.ContainerSize,
          ContainerType = input.ContainerType,
          CreatedBy = input.CreatedBy,
          CreatedDate = DateTime.Now,
          Id = Guid.NewGuid(),
          RowStatus = true,
          SuratPenyerahanPetikemasId = sp2Id,
          VesselName = input.VesselName,
          VesselNumber = input.VesselNumber,
          VoyageNumber = input.VoyageNumber
        };
        await Context.AddAsync(newEntity);
      }
    }

    public async Task<TrxDelegateDto> GetTrxDelegate(Guid Id)
    {
      try
      {
        var dorder = await Context.DeliveryOrderSet
          .Where(w => w.Id == Id && w.RowStatus == 1 &&
          w.ServiceName == ServiceType.DO.ToString())
          .SingleOrDefaultAsync();
        
        if (dorder == null)
        {
          var sp2 = await Context.SP2
          .Where(w => w.Id == Id && w.RowStatus == 1 &&
          w.ServiceName == ServiceType.SP2.ToString())
          .Include(i => i.Logs)
          .SingleOrDefaultAsync();

          var slog = await Context.SP2Log
          .Where(w => w.SuratPenyerahanPetikemasId == sp2.Id &&
          w.RowStatus)
          .ToListAsync();

          var logSp2 = slog == null ?
            new DelegateLog[] {} :
              slog.Select(s => new DelegateLog
              {
                CreatedDate = s.CreatedDate,
                PositionStatus = s.PositionStatus,
                PositionStatusName = s.PositionName
              }).ToArray();
          var resultSp2 = ToDelegate(sp2);
          resultSp2.Logs = logSp2;
          return resultSp2;
        }

        var dlog = await Context.DLogSet
        .Where(w => w.JobNumber == dorder.JobNumber &&
        w.RowStatus == 1)
        .ToListAsync();

        var logs = dlog == null ?
          new DelegateLog[] {} :
            dlog.Select(s => new DelegateLog
            {
              CreatedDate = s.CreatedDate,
              PositionStatus = PS(s.Activity),
              PositionStatusName = PSN(s.Activity)
            }).ToArray();
        var result = ToDelegate(dorder);
        result.Logs = logs;

        return result;
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<Tuple<IEnumerable<TrxDelegateList>, int>> GetTrxDelegates(
      TrxDelegateRequest request
    )
    {
      try
      {
        var orders = string.Join(',',
          request.Orders.Where(w => !string.IsNullOrWhiteSpace(w)));
        var queryDo = Context.DeliveryOrderSet
        .Where(w => w.RowStatus == 1 && w.ServiceName == ServiceType.DO.ToString());
        List<DeliveryOrder> doEntities;

        if (request.Status.HasValue)
        {
          int[] posStat = new int[] { 0 };
          if (request.Status == SP2Status.Actived)
          posStat = new int[] { 1, 2, 3, 4, 5 };
          if (request.Status == SP2Status.Completed)
          posStat = new int[] { 6 };
          queryDo = queryDo.Where(w => posStat.Contains(w.PositionStatus));
        }

        if (!string.IsNullOrWhiteSpace(request.CreatedBy))
        queryDo = queryDo.Where(w => w.CreatedBy.ToLower() == request.CreatedBy.ToLower());

        if (!string.IsNullOrWhiteSpace(request.JobNumber))
        queryDo = queryDo.Where(w => w.JobNumber.Contains(request.JobNumber));

        doEntities = await queryDo.ToListAsync();

        var querySp2 = Context.SP2
        .Where(w => w.RowStatus == 1 && w.ServiceName == ServiceType.SP2.ToString());
        List<SuratPenyerahanPetikemas> sp2Entities;

        if (request.Status.HasValue)
        {
          int[] posStat = new int[] { 0 };
          if (request.Status == SP2Status.Actived)
          posStat = new int[] { 1, 2, 3, 4 };
          if (request.Status == SP2Status.Completed)
          posStat = new int[] { 5 };
          querySp2 = querySp2.Where(w => posStat.Contains(w.PositionStatus));
        }

        if (!string.IsNullOrWhiteSpace(request.CreatedBy))
        querySp2 = querySp2.Where(w => w.CreatedBy.ToLower() == request.CreatedBy.ToLower());

        if (!string.IsNullOrWhiteSpace(request.JobNumber))
        querySp2 = querySp2.Where(w => w.JobNumber.Contains(request.JobNumber));

        sp2Entities = await querySp2.ToListAsync();

        var def = new List<TrxDelegateList>();
        var resultDo = doEntities == null ? def : doEntities.Select(DelegateList);
        def.AddRange(resultDo);
        var resultSp2 = sp2Entities == null ? def : sp2Entities.Select(DelegateList);
        def.AddRange(resultSp2);

        var count = def.Count;

        if (!string.IsNullOrWhiteSpace(orders))
        def = def.AsQueryable().OrderBy(orders).ToList();

        if (request.Length > 0)
        def = def.Take(request.Length).ToList();

        def = def.Skip(request.Start).ToList();

        return new Tuple<IEnumerable<TrxDelegateList>, int>(def, count);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    public async Task<CompanyOut> Company(Guid Id)
    {
      try
      {
        var entity = await Context.CompanySet
        .Where(w => w.Id == Id)
        .SingleOrDefaultAsync();
        
        var result = new CompanyOut();
        result.Changes(entity);
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
    Task<bool> CancelTransaction(Guid Id, string Reason);
    Task<SP2Detail> DetailSP2(Guid Id);
    Task<IEnumerable<ContractDto>> GetContracts(Guid? CompanyId);
    Task<IEnumerable<InvoiceDto>> GetInvoices();
    Task<IEnumerable<InvoiceDetailDto>> GetInvoiceDetails(Guid InvoiceId);
    Task<IEnumerable<RateContractDto>> GetRateContracts();
    Task<IEnumerable<RatePlatformList>> GetRatePlatforms();
    Task<IEnumerable<TransactionDto>> GetTransactions();
    Task<IEnumerable<TransactionTypeDto>> GetTransactionTypes();
    Task<Tuple<IEnumerable<SP2List>, int>> ListSP2(ListSP2Request request);
    Task<Tuple<IEnumerable<object>, int>> ListDoSp2(int start, int lenght,
      string createdBy, string forwarderName, bool? isDraft, bool? isDelegate);
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
    Task<string> PutTrxDelegate(DelegatePayload payload);
    Task<TrxDelegateDto> GetTrxDelegate(Guid Id);
    Task<Tuple<IEnumerable<TrxDelegateList>,int>> GetTrxDelegates(TrxDelegateRequest request);
    Task<ContractDto> Contract(Guid Id);
    Task<CompanyOut> Company(Guid Id);
  }
}