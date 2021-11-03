using System;
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
        public Guid Id { get; set; }
        public string KeyName { get; set; }
        public string Information { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    [Table("sp2")]
    public class SuratPenyerahanPetikemas
    {
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
}