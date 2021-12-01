using System;

namespace SP2.Data
{
  public class EmailDto
  {
    public string CustName { get; set; }
    public string CustEmail { get; set; }
    public string[] EmailCC { get; set; }
    public string TransNum { get; set; }
    public string GpUrl { get; set; }
  }

  public class InvoiceDetailDto
  {
    public Guid? Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid TransactionTypeId { get; set; }
    public double InvoiceAmount { get; set; }
    public bool RowStatus { get; set; }
  }

  public class InvoiceDto
  {
    public Guid? Id { get; set; }
    public Guid CompanyId { get; set; }
    public string InvoiceNumber { get; set; }
    public string InvoiceStatus { get; set; }
    public string JobNumber { get; set; }
    public bool IsContract { get; set; }
    public double TotalAmount { get; set; }
    public double DiscAmount { get; set; }
    public DateTime PaidThru { get; set; }
    public DateTime InvoiceDate { get; set; }
    public bool RowStatus { get; set; }
  }

  public class RateContractDto
  {
    public Guid? Id { get; set; }
    public Guid ContractId { get; set; }
    public Guid TransactionTypeId { get; set; }
    public double RateNominal { get; set; }
    public bool RowStatus { get; set; }
  }

  public class RatePlatformDto
  {
    public Guid? Id { get; set; }
    public Guid TransactionTypeId { get; set; }
    public double RateNominal { get; set; }
    public bool RowStatus { get; set; }
  }

  public class RatePlatformWithRelationDto : RatePlatformDto
  {
    public string TransactionAlias { get; set; }
  }

  public class TransactionDto
  {
    public Guid? Id { get; set; }
    public Guid TransactionTypeId { get; set; }
    public Guid CompanyId { get; set; }
    public string TransactionNumber { get; set; }
    public string JobNumber { get; set; }
    public bool Delegated { get; set; }
    public bool RowStatus { get; set; }
  }

  public class TransactionTypeDto
  {
    public Guid? Id { get; set; }
    public string TransactionAlias { get; set; }
    public string TransactionName { get; set; }
    public string TableName { get; set; }
    public bool RowStatus { get; set; }
  }

  public class SP2Dto
  {
    public Guid? Id { get; set; }
    public string CargoOwnerTaxId { get; set; }
    public string CargoOwnerName { get; set; }
    public string ForwarderTaxId { get; set; }
    public string ForwarderName { get; set; }
    public string TypeTransaction { get; set; }
    public string TerminalOperator { get; set; }
    public string JobNumber { get; set; }
    public string BLNumber { get; set; }
    public DateTime? BLDate { get; set; }
    public string DocumentType { get; set; }
    public DateTime? SPPBDate { get; set; }
    public string SPPBNumber { get; set; }
    public DateTime? PIBDate { get; set; }
    public string PIBNumber { get; set; }
    public DateTime? DODate { get; set; }
    public string DONumber { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsDraft { get; set; }
    public string PaymentMethod { get; set; }
    public string ProformaInvoiceNo { get; set; }
    public double SubTotalByThirdParty { get; set; }
    public double PlatformFee { get; set; }
    public double Vat { get; set; }
    public double GrandTotal { get; set; }
    public bool RowStatus { get; set; }
    public ContainerDto[] Containers { get; set; }
  }

  public class ContainerDto
  {
    public Guid? Id { get; set; }
    public Guid SP2Id { get; set; }
    public string BLNumber { get; set; }
    public string VesselNumber { get; set; }
    public string VesselName { get; set; }
    public string VoyageNumber { get; set; }
    public string ContainerNumber { get; set; }
    public string ContainerSize { get; set; }
    public string ContainerType { get; set; }
  }

  public class SP2List
  {
    public Guid Id { get; set; }
    public string JobNumber { get; set; }
    public string TypeTransaction { get; set; }
    public string TransactionName { get; set; }
    public string PaymentMethod { get; set; }
    public string StatusName { get; set; }
    public int StatusPosition { get; set; }
    public DateTime CreatedDate { get; set; }
  }

  public class DocumentDto
  {
    public string TerminalId { get; set; }
    public string TerminalOperator { get; set; }
    public string TransactionType { get; set; }
    public string TransactionName { get; set; }
    public string DocumentType { get; set; }
    public string DocumentName { get; set; }
    public string BLNumber { get; set; }

    public bool IsDraft { get; set; }
    public bool RowStatus { get; set; }

    public string JobNumber { get; set; }
    public DateTime? BLDate { get; set; }
    public string SPPBNumber { get; set; }
    public DateTime? SPPBDate { get; set; }
    public string PIBNumber { get; set; }
    public DateTime? PIBDate { get; set; }
    public string DONumber { get; set; }
    public DateTime? DODate { get; set; }
  }
}