using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SP2.Data
{
  public class BaseEntity
  {
    public Guid Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
    public byte RowStatus { get; set; }
  }

  [Table("KOJA")]
  public class Koja
  {
    [Key]
    public Guid Id { get; set; }
    public string KeyName { get; set; }
    public string Information { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
  }

  [Table("DummyDocument")]
  public class Document
  {
    [Key]
    public Guid Id { get; set; }
    public string TerminalId { get; set; }
    public string TerminalName { get; set; }
    public string TransactionType { get; set; }
    public string TransactionName { get; set; }
    public string DocumentCode { get; set; }
    public string DocumentName { get; set; }
    public string BLNumber { get; set; }
    
    public string JobNumber { get; set; }
    public DateTime? BLDate { get; set; }
    public string SPPBNumber { get; set; }
    public DateTime? SPPBDate { get; set; }
    public string PIBNumber { get; set; }
    public DateTime? PIBDate { get; set; }
    public string DONumber { get; set; }
    public DateTime? DODate { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool RowStatus { get; set; }
  }

  [Table("SP2")]
  public class SuratPenyerahanPetikemas
  {
    [Key]
    public Guid Id { get; set; }
    public string TerminalId { get; set; }
    [Column("TerminalName")] public string TerminalOperator { get; set; }
    [Column("TransactionType")] public string TypeTransaction { get; set; }
    public string TransactionName { get; set; }
    [Column("DocumentCode")] public string DocumentType { get; set; }
    public string DocumentName { get; set; }
    public string BLNumber { get; set; }
    
    public string JobNumber { get; set; }
    public DateTime? BLDate { get; set; }
    public string SPPBNumber { get; set; }
    public DateTime? SPPBDate { get; set; }
    public string PIBNumber { get; set; }
    public DateTime? PIBDate { get; set; }
    public string DONumber { get; set; }
    public DateTime? DODate { get; set; }

    public int PositionStatus { get; set; }
    public string PaymentMethod { get; set; }

    public string CargoOwnerTaxId { get; set; }
    public string CargoOwnerName { get; set; }
    public string ForwarderTaxId { get; set; }
    public string ForwarderName { get; set; }
    public DateTime DueDate { get; set; }
    public string ProformaInvoiceNo { get; set; }
    public string ProformaInvoiceUrl { get; set; }
    public double SubTotalByThirdParty { get; set; }
    public double PlatformFee { get; set; }
    public double Vat { get; set; }
    public double GrandTotal { get; set; }
    public string CancelReason { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool RowStatus { get; set; }

    [Column("IsDraft")]
    public bool SaveAsDraft { get; set; }
    public string ServiceName { get; set; }
    public string ContractNumber { get; set; }
    public string FrieghtForwarderName { get; set; }
    [Column("BillOfLadingFile")]
    public string BLDocument { get; set; }
    [Column("LetterOfIndemnityFile")]
    public string LetterOfIndemnity { get; set; }
    [Column("AttorneyLetterFile")]
    public string AttorneyLetter { get; set; }
    public string PositionStatusName { get; set; }
    [Column("NoticeEmail")]
    public string NotifyEmails { get; set; }

    public virtual IEnumerable<Container> Containers { get; set; }
    public virtual IEnumerable<Log> Logs { get; set; }
    public virtual IEnumerable<Notify> Notifies { get; set; }
  }

  [Table("SP2Container")]
  public class Container
  {
    [Key]
    public Guid Id { get; set; }
    [Column("SP2Id")]
    public Guid SuratPenyerahanPetikemasId { get; set; }
    public string BLNumber { get; set; }
    public string VesselNumber { get; set; }
    public string VesselName { get; set; }
    public string VoyageNumber { get; set; }
    public string ContainerNumber { get; set; }
    public string ContainerSize { get; set; }
    public string ContainerType { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool RowStatus { get; set; }
  }

  [Table("SP2Log")]
  public class Log
  {
    [Key]
    public Guid Id { get; set; }
    [Column("SP2Id")]
    public Guid SuratPenyerahanPetikemasId { get; set; }
    public int PositionStatus { get; set; }
    public string PositionName { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool RowStatus { get; set; }
  }

  [Table("SP2Notify")]
  public class Notify
  {
    [Key]
    public Guid Id { get; set; }
    [Column("SP2Id")]
    public Guid SuratPenyerahanPetikemasId { get; set; }
    public string Email { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool RowStatus { get; set; }
  }

  [Table("Companies")]
  public class Company
  {
    [Key]
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

  /// Master Contract
  [Table("Contract")]
  public class Contract
  {
    [Key]
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string ContractNumber { get; set; }
    public string FirstParty { get; set; }
    public string SecondParty { get; set; }
    public string Services { get; set; }
    public string BillingPeriod { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public double? PriceRate { get; set; }

    #region system need
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    #endregion

    public bool RowStatus { get; set; }
  }

  /// Master Transaction Type
  [Table("TransactionType")]
  public class TransactionType
  {
    [Key]
    public Guid Id { get; set; }
    public string TransactionAlias { get; set; }
    public string TransactionName { get; set; }
    public string TableName { get; set; }

    #region system need
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    #endregion

    public bool RowStatus { get; set; }
  }

  [Table("RateContract")]
  public class RateContract
  {
    [Key]
    public Guid Id { get; set; }
    public Guid ContractId { get; set; }
    public Guid TransactionTypeId { get; set; }
    public double RateNominal { get; set; }

    #region system need
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    #endregion

    public bool RowStatus { get; set; }
  }

  [Table("RatePlatformFee")]
  public class RatePlateformFee
  {
    [Key]
    public Guid Id { get; set; }
    public Guid TransactionTypeId { get; set; }
    public double RateNominal { get; set; }

    #region system need
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    #endregion

    public bool RowStatus { get; set; }
    public virtual TransactionType TrxType { get; set; }
  }

  [Table("Transaction")]
  public class Transaction
  {
    [Key]
    public Guid Id { get; set; }
    public Guid TransactionTypeId { get; set; }
    public Guid CompanyId { get; set; }
    public string TransactionNumber { get; set; }
    public string JobNumber { get; set; }
    public bool Delegated { get; set; }
    public string CancelReason { get; set; }

    #region system need
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    #endregion

    public bool RowStatus { get; set; }
  }

  [Table("InvoicePlatformFee")]
  public class InvoicePlatformFee
  {
    [Key]
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string InvoiceNumber { get; set; }
    public string InvoiceStatus { get; set; }
    public string JobNumber { get; set; }
    public bool IsContract { get; set; }
    public double TotalAmount { get; set; }
    public double DiscAmount { get; set; }
    public DateTime PaidThru { get; set; }

    #region system need
    public string CreatedBy { get; set; }
    /// use as InvoiceDate
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    #endregion

    public bool RowStatus { get; set; }
  }

  [Table("InvoiceDetailPlatformFee")]
  public class InvoiceDetailPlatformFee
  {
    [Key]
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid TransactionTypeId { get; set; }
    public double InvoiceAmount { get; set; }

    #region system need
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    #endregion

    public bool RowStatus { get; set; }
  }

  [Table("DeliveryOrders")]
  public class DeliveryOrder
  {
    [Key]
    public Guid Id { get; set; }
    [StringLength(64)]
    public string DeliveryOrderType { get; set; }
    [StringLength(64)]
    public string DeliveryOrderNumber { get; set; }
    public DateTime? DeliveryOrderExpiredDate { get; set; }
    [StringLength(64)]
    public string DeliveryOrderStatus { get; set; }

    [StringLength(258)]
    public string JobNumber { get; set; }

    [Required]
    [StringLength(64)]
    public string CustomerCode { get; set; }
    [Required]
    [StringLength(256)]
    public string CustomerName { get; set; }

    [Required]
    [StringLength(64)]
    public string BillOfLadingNumber { get; set; }
    [Required]
    public DateTime BillOfLadingDate { get; set; }

    [Required]
    [StringLength(256)]
    public string ShippingLineName { get; set; }
    [Required]
    [StringLength(512)]
    public string ShippingLineEmail { get; set; }
    [Required]
    [StringLength(512)]
    public string Vessel { get; set; }
    [Required]
    [StringLength(512)]
    public string VoyageNumber { get; set; }
    [Required]
    [StringLength(512)]
    public string Consignee { get; set; }
    [Required]
    [StringLength(512)]
    public string PortOfLoading { get; set; }
    [Required]
    [StringLength(512)]
    public string PortOfDischarge { get; set; }
    [Required]
    [StringLength(512)]
    public string PortOfDelivery { get; set; }
    [Required]
    [StringLength(512)]
    public string NotifyPartyName { get; set; }
    [Required]
    [StringLength(512)]
    public string NotifyPartyAdress { get; set; }
    public Guid? CustomerID { get; set; }
    public int PositionStatus { get; set; }
    public string Shipper { get; set; }
    public string NoPos { get; set; }
    public long ProformaInvoiceAmount { get; set; }

    public byte RowStatus { get; set; }

    [Required]
    [StringLength(128)]
    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    [StringLength(128)]
    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [Column("IsDraft")]
    public bool SaveAsDraft { get; set; }
    public string ServiceName { get; set; }
    public string ContractNumber { get; set; }
    public string FrieghtForwarderName { get; set; }
    [Column("BillOfLadingFile")]
    public string BLDocument { get; set; }
    [Column("LetterOfIndemnityFile")]
    public string LetterOfIndemnity { get; set; }
    [Column("AttorneyLetterFile")]
    public string AttorneyLetter { get; set; }
    public string PositionStatusName { get; set; }
    [Column("NoticeEmail")]
    public string NotifyEmails { get; set; }

    public virtual IEnumerable<DLog> Logs { get; set; }
  }

  [Table("Invoices")]
  public class JustInvoice
  {
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid CargoOwnerId { get; set; }
    public Guid ServiceId { get; set; }
    public string InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string JobNumber { get; set; }
    public string ProformaInvoiceNumber { get; set; }
    public DateTime ProformaInvoiceDate { get; set; }
    public string FakturPajakNumber { get; set; }
    public string BLNumber { get; set; }
    public Guid ShippingLineId { get; set; }
    public string DONumber { get; set; }
    public int Qty { get; set; }
    public string Description { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }
    public decimal VAT { get; set; }
    public decimal ShipplingLineHandlingFee { get; set; }
    public decimal ServiceFee { get; set; }
    public decimal Total { get; set; }
    public int PaymentStatusId { get; set; }
    public string PDFId { get; set; }
    public string PDFName { get; set; }
    public byte RowStatus { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
  }

  [Table("TransactionDelegate")]
  public class TrxDelegate
  {
    [Key]
    public Guid Id { get; set; }
    [Column("IsDraft")]
    public bool SaveAsDraft { get; set; }
    public string ServiceName { get; set; }
    public string ContractNumber { get; set; }
    public string JobNumber { get; set; }
    public string FrieghtForwarderName { get; set; }
    [Column("BillOfLadingFile")]
    public string BLDocument { get; set; }
    [Column("LetterOfIndemnityFile")]
    public string LetterOfIndemnity { get; set; }
    [Column("AttorneyLetterFile")]
    public string AttorneyLetter { get; set; }
    public int PositionStatus { get; set; }
    public string PositionStatusName { get; set; }
    public string NotifyEmails { get; set; }
    
    public byte RowStatus { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
  }

  [Table("DeliveryOrderLogs")]
  public class DLog
  {
    [Key] public Guid Id { get; set; }
    [StringLength(256)] public string JobNumber { get; set; }
    [Required, StringLength(64)] public string BillOfLadingNumber { get; set; }
    [StringLength(64)] public string Activity { get; set; }
    [Required, StringLength(128)] public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    [StringLength(128)] public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public byte RowStatus { get; set; }
  }
}