using System.Linq;

namespace SP2.Data
{
  public class Mapping
  {
    public InvoiceDetailDto To(InvoiceDetailPlatformFee entity)
    {
      return new InvoiceDetailDto
      {
        Id = entity.Id,
        InvoiceAmount = entity.InvoiceAmount,
        InvoiceId = entity.InvoiceId,
        RowStatus = entity.RowStatus,
        TransactionTypeId = entity.TransactionTypeId
      };
    }

    public InvoiceDto To(InvoicePlatformFee entity)
    {
      return new InvoiceDto
      {
        CompanyId = entity.CompanyId,
        DiscAmount = entity.DiscAmount,
        Id = entity.Id,
        InvoiceDate = entity.CreatedDate,
        InvoiceNumber = entity.InvoiceNumber,
        InvoiceStatus = entity.InvoiceStatus,
        IsContract = entity.IsContract,
        JobNumber = entity.JobNumber,
        PaidThru = entity.PaidThru,
        RowStatus = entity.RowStatus,
        TotalAmount = entity.TotalAmount
      };
    }

    public RateContractDto To(RateContract entity)
    {
      return new RateContractDto
      {
        ContractId = entity.ContractId,
        Id = entity.Id,
        RateNominal = entity.RateNominal,
        RowStatus = entity.RowStatus,
        TransactionTypeId = entity.TransactionTypeId
      };
    }

    public RatePlatformList To(RatePlateformFee entity)
    {
      return new RatePlatformList
      {
        Id = entity.Id,
        RateNominal = entity.RateNominal,
        RowStatus = entity.RowStatus,
        TransactionAlias = entity.TrxType.TransactionAlias,
        TransactionTypeId = entity.TransactionTypeId
      };
    }

    public TransactionDto To(Transaction entity)
    {
      return new TransactionDto
      {
        CompanyId = entity.CompanyId,
        Delegated = entity.Delegated,
        Id = entity.Id,
        JobNumber = entity.JobNumber,
        RowStatus = entity.RowStatus,
        TransactionNumber = entity.TransactionNumber,
        TransactionTypeId = entity.TransactionTypeId
      };
    }

    public TransactionTypeDto To(TransactionType entity)
    {
      return new TransactionTypeDto
      {
        Id = entity.Id,
        RowStatus = entity.RowStatus,
        TableName = entity.TableName,
        TransactionAlias = entity.TransactionAlias,
        TransactionName = entity.TransactionName
      };
    }

    public SP2Detail To(SuratPenyerahanPetikemas entity)
    {
      var containers = entity.Containers == null ?
        new ContainerDto[] {} :
          entity.Containers.Select(To).ToArray();
      var logs = entity.Logs == null ?
        new LogDto[] {} :
          entity.Logs.Select(To).ToArray();
      var notifies = entity.Notifies == null ?
        new NotifyDto[] {} :
          entity.Notifies.Select(To).ToArray();
      return new SP2Detail
      {
        BLDate = entity.BLDate,
        BLNumber = entity.BLNumber,
        DocumentType = entity.DocumentType,
        DODate = entity.DODate,
        DONumber = entity.DONumber,
        Id = entity.Id,
        JobNumber = entity.JobNumber,
        PIBDate = entity.PIBDate,
        PIBNumber = entity.PIBNumber,
        SPPBDate = entity.SPPBDate,
        SPPBNumber = entity.SPPBNumber,
        RowStatus = entity.RowStatus,
        TerminalOperator = entity.TerminalOperator,
        TypeTransaction = entity.TypeTransaction,

        CargoOwnerName = entity.CargoOwnerName,
        CargoOwnerTaxId = entity.CargoOwnerTaxId,
        Containers = containers,
        CreatedDate = entity.CreatedDate,
        DueDate = entity.DueDate,
        ForwarderName = entity.ForwarderName,
        ForwarderTaxId = entity.ForwarderTaxId,
        GrandTotal = entity.GrandTotal,
        IsDraft = entity.PositionStatus == 0,
        Logs = logs,
        Notifies = notifies,
        PaymentMethod = entity.PaymentMethod,
        PlatformFee = entity.PlatformFee,
        ProformaInvoiceNo = entity.ProformaInvoiceNo,
        StatusPosition = entity.PositionStatus,
        SubTotalByThirdParty = entity.SubTotalByThirdParty,
        Vat = entity.Vat
      };
    }

    public ContainerDto To(Container entity)
    {
      return new ContainerDto
      {
        BLNumber = entity.BLNumber,
        ContainerNumber = entity.ContainerNumber,
        ContainerSize = entity.ContainerSize,
        ContainerType = entity.ContainerType,
        Id = entity.Id,
        VesselName = entity.VesselName,
        VesselNumber = entity.VesselNumber,
        VoyageNumber = entity.VoyageNumber
      };
    }

    public LogDto To(Log entity)
    {
      return new LogDto
      {
        Id = entity.Id,
        PositionName = entity.PositionName,
        PositionStatus = entity.PositionStatus,
        CreatedDate = entity.CreatedDate
      };
    }

    public NotifyDto To(Notify entity)
    {
      return new NotifyDto
      {
        Email = entity.Email,
        Id = entity.Id
      };
    }

    public SP2List ToList(SuratPenyerahanPetikemas entity)
    {
      string positionName;
      switch (entity.PositionStatus)
      {
        case 1: positionName = "Request Form"; break;
        case 2: positionName = "Proforma Invoice"; break;
        case 3: positionName = "Payment & Confirmation"; break;
        case 4: positionName = "SP2 & Invoice Release"; break;
        default: positionName = "Draft"; break;
      }

      return new SP2List
      {
        CreatedDate = entity.CreatedDate,
        CompletedDate = entity.PositionStatus == 4 ? entity.ModifiedDate : null,
        Id = entity.Id,
        JobNumber = entity.JobNumber,
        PaymentMethod = entity.PaymentMethod,
        StatusPosition = entity.PositionStatus,
        StatusName = positionName,
        TransactionName = entity.TransactionName,
        TypeTransaction = entity.TypeTransaction
      };
    }
  }
}