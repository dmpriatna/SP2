using System.Linq;

namespace SP2.Data
{
  public class Mapping
  {
    public ContractDto To(Contract entity)
    {
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
        ModifiedDate = entity.ModifiedDate,
        PaymentMethod = entity.PaymentMethod,
        PlatformFee = entity.PlatformFee,
        ProformaInvoiceNo = entity.ProformaInvoiceNo,
        ProformaInvoiceUrl = entity.ProformaInvoiceUrl,
        StatusPosition = entity.PositionStatus,
        SubTotalByThirdParty = entity.SubTotalByThirdParty,
        Vat = entity.Vat,

        CreatedBy = entity.CreatedBy,
        IsDelegate = !string.IsNullOrWhiteSpace(entity.ServiceName)
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
      string positionName = "Draft";
      if (string.IsNullOrWhiteSpace(entity.ServiceName))
      {
        switch (entity.PositionStatus)
        {
          case 1: positionName = "Request Form"; break;
          case 2: positionName = "Proforma Invoice"; break;
          case 3: positionName = "Payment & Confirmation"; break;
          case 4: positionName = "SP2 & Invoice Release"; break;
          default: positionName = "Draft"; break;
        }
      }
      else
      {
        positionName = entity.PositionStatusName;
      }

      return new SP2List
      {
        CreatedDate = entity.CreatedDate,
        CompletedDate = entity.PositionStatus == 4 ? entity.ModifiedDate : null,
        Id = entity.Id,
        IsDelegate = entity.ServiceName != null,
        JobNumber = entity.JobNumber,
        PaymentMethod = entity.PaymentMethod,
        StatusPosition = entity.PositionStatus,
        StatusName = positionName,
        TransactionName = entity.TransactionName,
        TypeTransaction = entity.TypeTransaction
      };
    }

    public DoSp2Dto To(DeliveryOrder entity)
    {
      return new DoSp2Dto
      {
        BillOfLadingDate = entity.BillOfLadingDate,
        BillOfLadingNumber = entity.BillOfLadingNumber,
        Consignee = entity.Consignee,
        CreatedBy = entity.CreatedBy,
        CreatedDate = entity.CreatedDate,
        CustomerCode = entity.CustomerCode,
        CustomerID = entity.CustomerID,
        CustomerName = entity.CustomerName,
        DeliveryOrderExpiredDate = entity.DeliveryOrderExpiredDate,
        DeliveryOrderNumber = entity.DeliveryOrderNumber,
        DeliveryOrderStatus = entity.DeliveryOrderStatus,
        DeliveryOrderType = entity.DeliveryOrderType,
        Id = entity.Id,
        JobNumber = entity.JobNumber,
        ModifiedBy = entity.ModifiedBy,
        ModifiedDate = entity.ModifiedDate,
        NoPos = entity.NoPos,
        NotifyPartyAdress = entity.NotifyPartyAdress,
        NotifyPartyName = entity.NotifyPartyName,
        PortOfDelivery = entity.PortOfDelivery,
        PortOfDischarge = entity.PortOfDischarge,
        PortOfLoading = entity.PortOfLoading,
        PositionStatus = entity.PositionStatus,
        ProformaInvoiceAmount = entity.ProformaInvoiceAmount,
        RowStatus = entity.RowStatus,
        Shipper = entity.Shipper,
        ShippingLineEmail = entity.ShippingLineEmail,
        ShippingLineName = entity.ShippingLineName,
        Vessel = entity.Vessel,
        VoyageNumber = entity.VoyageNumber,

        IsDelegate = !string.IsNullOrWhiteSpace(entity.ServiceName)
      };
    }

    public TrxDelegateDto ToDelegate(DeliveryOrder entity)
    {
      if (entity == null) return new TrxDelegateDto();
      var notifies = entity.NotifyEmails == null ?
        new string[] {} :
          entity.NotifyEmails.Split(';');
      return new TrxDelegateDto
      {
        AttorneyLetter = entity.AttorneyLetter,
        BLDocument = entity.BLDocument,
        ContractNumber = entity.ContractNumber,
        CreatedBy = entity.CreatedBy,
        CreatedDate = entity.CreatedDate,
        FrieghtForwarderName = entity.FrieghtForwarderName,
        Id = entity.Id,
        JobNumber = entity.JobNumber,
        LetterOfIndemnity = entity.LetterOfIndemnity,
        ModifiedBy = entity.ModifiedBy,
        ModifiedDate = entity.ModifiedDate,
        NotifyEmails = notifies,
        PositionStatus = entity.PositionStatus,
        PositionStatusName = entity.PositionStatusName,
        RowStatus = entity.RowStatus,
        SaveAsDraft = entity.SaveAsDraft,
        ServiceName = entity.ServiceName
      };
    }

    public int PS(string source)
    {
      var first = source.Split(';').First();
      var tried = int.TryParse(first, out int result);
      if (tried) return result;
      return 0;
    }

    public string PSN(string source)
    {
      return source.Split(';').Last();
    }

    public TrxDelegateDto ToDelegate(SuratPenyerahanPetikemas entity)
    {
      if (entity == null) return new TrxDelegateDto();
      var notifies = entity.NotifyEmails == null ?
        new string[] {} :
          entity.NotifyEmails.Split(';');
      var logs = entity.Logs == null ?
        new DelegateLog[] {} :
          entity.Logs.Select(s => new DelegateLog
          {
            CreatedDate = s.CreatedDate,
            PositionStatus = s.PositionStatus,
            PositionStatusName = s.PositionName
          }).ToArray();
      return new TrxDelegateDto
      {
        AttorneyLetter = entity.AttorneyLetter,
        BLDocument = entity.BLDocument,
        ContractNumber = entity.ContractNumber,
        CreatedBy = entity.CreatedBy,
        CreatedDate = entity.CreatedDate,
        FrieghtForwarderName = entity.FrieghtForwarderName,
        Id = entity.Id,
        JobNumber = entity.JobNumber,
        LetterOfIndemnity = entity.LetterOfIndemnity,
        Logs = logs,
        ModifiedBy = entity.ModifiedBy,
        ModifiedDate = entity.ModifiedDate,
        NotifyEmails = notifies,
        PositionStatus = entity.PositionStatus,
        PositionStatusName = entity.PositionStatusName,
        RowStatus = entity.RowStatus,
        SaveAsDraft = entity.SaveAsDraft,
        ServiceName = entity.ServiceName
      };
    }

    public TrxDelegateList DelegateList(DeliveryOrder entity)
    {
      return new TrxDelegateList
      {
        CreatedDate = entity.ModifiedDate.HasValue ? entity.ModifiedDate.Value : entity.CreatedDate,
        Id = entity.Id,
        JobNumber = entity.JobNumber,
        PositionStatus = entity.PositionStatus,
        ServiceName = entity.ServiceName
      };
    }

    public TrxDelegateList DelegateList(SuratPenyerahanPetikemas entity)
    {
      return new TrxDelegateList
      {
        CreatedDate = entity.ModifiedDate.HasValue ? entity.ModifiedDate.Value : entity.CreatedDate,
        Id = entity.Id,
        JobNumber = entity.JobNumber,
        PositionStatus = entity.PositionStatus,
        ServiceName = entity.ServiceName
      };
    }
  }
}