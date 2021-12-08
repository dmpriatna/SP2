using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SP2.Data
{
  public class EmailFbr
  {
    public string CustName { get; set; }
    public string CustEmail { get; set; }
    public string[] EmailCC { get; set; }
    public string TransNum { get; set; }
    public string GpUrl { get; set; }
  }

  public class InvoiceDetailFbr
  {
    public Guid? Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid TransactionTypeId { get; set; }
    public double InvoiceAmount { get; set; }
    public bool RowStatus { get; set; }
  }

  public class InvoiceFbr
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

  public class RateContractFbr
  {
    public Guid? Id { get; set; }
    public Guid ContractId { get; set; }
    public Guid TransactionTypeId { get; set; }
    public double RateNominal { get; set; }
    public bool RowStatus { get; set; }
  }

  public class RatePlatformFbr
  {
    public Guid? Id { get; set; }
    public Guid TransactionTypeId { get; set; }
    public double RateNominal { get; set; }
    public bool RowStatus { get; set; }
  }

  public class TransactionFbr
  {
    public Guid? Id { get; set; }
    public Guid TransactionTypeId { get; set; }
    public Guid CompanyId { get; set; }
    public string TransactionNumber { get; set; }
    public string JobNumber { get; set; }
    public bool Delegated { get; set; }
    public bool RowStatus { get; set; }
  }

  public class TransactionTypeFbr
  {
    public Guid? Id { get; set; }
    public string TransactionAlias { get; set; }
    public string TransactionName { get; set; }
    public string TableName { get; set; }
    public bool RowStatus { get; set; }
  }

  public class SP2Fbr
  {
    public Guid? Id { get; set; }
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
  }

  public class ListSP2Request
  {
    public int Start { get; set; }
    public int Length { get; set; }
    public bool? IsDraft { get; set; }
    public string Search { get; set; }
    public int Status { get; set; }
    public string PaymentMethod { get; set; }
    public string[] Orders { get; set; }
  }

  [JsonConverter(typeof(StringEnumConverter))]
  public enum SP2Status
  {
    [EnumMember(Value = "Actived")] Actived,
    [EnumMember(Value = "Draft")] Draft,
    [EnumMember(Value = "Completed")] Completed
  }

  public class SP2StatusRequest
  {
    public Guid? Id { get; set; }
    public string JobNumber { get; set; }
    public SP2StatusR Status { get; set; }
  }

  [JsonConverter(typeof(StringEnumConverter))]
  public enum SP2StatusR
  {
    [EnumMember(Value = "Draft")] Draft,
    [EnumMember(Value = "RequestForm")] RequestForm,
    [EnumMember(Value = "ProformaInvoice")] ProformaInvoice,
    [EnumMember(Value = "PaymentConfirmation")] PaymentConfirmation,
    [EnumMember(Value = "SP2InvoiceRelease")] SP2InvoiceRelease
   }
}