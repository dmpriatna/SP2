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
}