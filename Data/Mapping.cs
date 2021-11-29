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

    public RatePlatformDto To(RatePlateformFee entity)
    {
      return new RatePlatformDto
      {
        Id = entity.Id,
        RateNominal = entity.RateNominal,
        RowStatus = entity.RowStatus,
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

    public SP2Dto To(SuratPenyerahanPetikemas entity)
    {
      return new SP2Dto
      {
        BLDate = entity.BLDate,
        BLNumber = entity.BLNumber,
        DocumentCode = entity.DocumentCode,
        DocumentName = entity.DocumentName,
        DODate = entity.DODate,
        DONumber = entity.DONumber,
        Id = entity.Id,
        IsDraft = entity.IsDraft,
        JobNumber = entity.JobNumber,
        PIBDate = entity.PIBDate,
        PIBNumber = entity.PIBNumber,
        SPPBDate = entity.SPPBDate,
        SPPBNumber = entity.SPPBNumber,
        RowStatus = entity.RowStatus,
        TerminalId = entity.TerminalId,
        TerminalName = entity.TerminalName,
        TransactionName = entity.TransactionName,
        TransactionType = entity.TransactionType
      };
    }
  }
}