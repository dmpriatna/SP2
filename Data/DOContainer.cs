using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SP2.Data
{
    [Table("DeliveryOrderContainers")]
    public class DOContainer : BaseEntity
    {
        public Guid DeliveryOrderId { get; set; }
        public string ContainerNo { get; set; }
        public int ContainerSize { get; set; }
        public string ContainerType { get; set; }
        public string DepoName { get; set; }
        public string SealNo { get; set; }
        public int GrossWeight { get; set; }
        public string PhoneNumber { get; set; }
    }
}