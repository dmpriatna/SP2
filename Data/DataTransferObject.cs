using System;

namespace SP2.Data
{
  public interface IDataTransferObject
  {
    DateTime CreatedDate { get; set; }
    DateTime? ModifiedDate { get; set; }
  }

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

  public class RatePlatformList
  {
    public Guid? Id { get; set; }
    public Guid TransactionTypeId { get; set; }
    public string TransactionAlias { get; set; }
    public double RateNominal { get; set; }
    public bool RowStatus { get; set; }
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
    public string ProformaInvoiceUrl { get; set; }
    public double SubTotalByThirdParty { get; set; }
    public double PlatformFee { get; set; }
    public double Vat { get; set; }
    public double GrandTotal { get; set; }
    public byte RowStatus { get; set; }
    public ContainerDto[] Containers { get; set; }
    public NotifyDto[] Notifies { get; set; }

    public string CreatedBy { get; set; }
  }

  public class ContainerDto
  {
    public Guid? Id { get; set; }
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
    public DateTime? CompletedDate { get; set; }
    public bool IsDelegate { get; set; }
  }
  
  public class SP2Detail : IDataTransferObject
  {
    public Guid? Id { get; set; }
    public string CargoOwnerTaxId { get; set; }
    public string CargoOwnerName { get; set; }
    public string ForwarderTaxId { get; set; }
    public string ForwarderName { get; set; }
    public string TypeTransaction { get; set; }
    public string TerminalOperator { get; set; }
    public int StatusPosition { get; set; }
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
    public bool IsDelegate { get; set; }
    public bool IsDraft { get; set; }
    public string PaymentMethod { get; set; }
    public string ProformaInvoiceNo { get; set; }
    public string ProformaInvoiceUrl { get; set; }
    public double SubTotalByThirdParty { get; set; }
    public double PlatformFee { get; set; }
    public double Vat { get; set; }
    public double GrandTotal { get; set; }
    public byte RowStatus { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public ContainerDto[] Containers { get; set; }
    public NotifyDto[] Notifies { get; set; }
    public LogDto[] Logs { get; set; }
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

  public class LogDto
  {
    public Guid? Id { get; set; }
    private int pos;
    public int PositionStatus
    {
      get { return pos; }
      set
      {
        switch (value)
        {
          case 1: PositionName = "Request Form"; break;
          case 2: PositionName = "Proforma Invoice"; break;
          case 3: PositionName = "Payment & Confirmation"; break;
          case 4: PositionName = "SP2 & Invoice Release"; break;
          default: PositionName = "Draft"; break;
        }
        pos = value;
      }
    }
    public string PositionName { get; set; }
    public DateTime? CreatedDate { get; set; }
  }

  public class NotifyDto
  {
    public Guid? Id { get; set; }
    public string Email { get; set; }
  }

  public class ContractDto
  {
    public Guid? Id { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? CargoOwnerId { get; set; }
    public string EmailPPJK { get; set; }
    public string ContractNumber { get; set; }
    public string FirstParty { get; set; }
    public string SecondParty { get; set; }
    public string Services { get; set; }
    public string BillingPeriod { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public double? PriceRate { get; set; }
    public bool RowStatus { get; set; }
  }

  public class DoSp2Dto : IDataTransferObject
  {
    public Guid Id { get; set; }

    public string DeliveryOrderType { get; set; }
    public string DeliveryOrderNumber { get; set; }
    public DateTime? DeliveryOrderExpiredDate { get; set; }
    public string DeliveryOrderStatus { get; set; }
    public string JobNumber { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public string BillOfLadingNumber { get; set; }
    public DateTime BillOfLadingDate { get; set; }
    public string ShippingLineName { get; set; }
    public string ShippingLineEmail { get; set; }
    public string Vessel { get; set; }
    public string VoyageNumber { get; set; }
    public string Consignee { get; set; }
    public string PortOfLoading { get; set; }
    public string PortOfDischarge { get; set; }
    public string PortOfDelivery { get; set; }
    public string NotifyPartyName { get; set; }
    public string NotifyPartyAdress { get; set; }
    public Guid? CustomerID { get; set; }
    public int PositionStatus { get; set; }
    public string Shipper { get; set; }
    public string NoPos { get; set; }
    public long ProformaInvoiceAmount { get; set; }

    public byte RowStatus { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public bool IsDelegate { get; set; }
  }

  public class TrxDelegateDto
  {
    public Guid? Id { get; set; }
    public bool SaveAsDraft { get; set; }
    public string ServiceName { get; set; }
    public string BLNumber { get; set; }
    public string ContractNumber { get; set; }
    public string JobNumber { get; set; }
    public string FrieghtForwarderName { get; set; }
    public string BLDocument { get; set; }
    public string LetterOfIndemnity { get; set; }
    public string AttorneyLetter { get; set; }
    public int PositionStatus { get; set; }
    public string PositionStatusName { get; set; }
    public string[] NotifyEmails { get; set; }
    public DelegateLog[] Logs { get; set; }
    
    public byte RowStatus { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
  }

  public class DelegateLog
  {
    public int PositionStatus { get; set; }
    public string PositionStatusName { get; set; }
    public DateTime CreatedDate { get; set; }
  }

  public class TrxDelegateList
  {
    public Guid Id { get; set; }
    public string JobNumber { get; set; }
    public string ServiceName { get; set; }
    public int PositionStatus { get; set; }
    public DateTime CreatedDate { get; set; }
  }

  public class DelegateContainerInput
  {
    // both Delegate have
    public Guid? Id { get; set; }
    public string ContainerNumber { get; set; }
    public string ContainerSize { get; set; }
    public string ContainerType { get; set; }
    public string CreatedBy { get; set; }

    // Delegate Delivery Order
    public string SealNumber { get; set; }
    public decimal? GrossWeight { get; set; }
    public string DepoName { get; set; }
    public string PhoneNumber { get; set; }
    public string LoadType { get; set; }

    // Delegate Surat Penyerahan Petikemas
    public string BLNumber { get; set; }
    public string VesselName { get; set; }
    public string VesselNumber { get; set; }
    public string VoyageNumber { get; set; }    
  }

  public class CompanyOut
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string NIB { get; set; }
    public string SubDistrict { get; set; }
    public string Address { get; set; }
    public string Province { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string NPWP { get; set; }
    public string Type { get; set; }
    public Guid? PersonID { get; set; }
    public Guid CreatedById { get; set; }
    public Guid ModifiedById { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool RowStatus { get; set; }
  }
}