using System;

namespace SP2.Data
{
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
    public Guid RateContractId { get; set; }
    public Guid TransactionTypeId { get; set; }
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
    public string TransactionName { get; set; }
    public string TableName { get; set; }
    public bool RowStatus { get; set; }
  }
}