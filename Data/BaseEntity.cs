using System;
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

    [Table("SP2")]
    public class SuratPenyerahanPetikemas
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
        
        public DateTime BLDate { get; set; }
        public string SPPBNumber { get; set; }
        public DateTime SPPBDate { get; set; }
        public string PIBNumber { get; set; }
        public DateTime PIBDate { get; set; }
        public string DONumber { get; set; }
        public DateTime DODate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
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
        public string PersonID { get; set; }
        public string CreatedById { get; set; }
        public string ModifiedById { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool RowStatus { get; set; }
    }

    /// Master Contract
    public class Contract
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid PersonId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

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
        public Guid RateContractId { get; set; }
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
}