using System.ComponentModel.DataAnnotations;

namespace SP2.Models.Main
{
  public class DataRequest
  {
    [Required]
    public string Creator { get; set; }
  }

  public class DocNTransRequest : DataRequest
  {
    public DocNTrans Request { get; set; }
  }

  public class DocNTrans
  {
    public string CATEGORY_ID { get; set; } = "I";
    public string TERMINAL_ID { get; set; } = "KOJA";
    public string GROUP_ID { get; set; } = "11";
  }

  public class DocNGenRequest : DataRequest
  {
    public DocNGen Request { get; set; }
  }

  public class DocNGen
  {
    public string NPWP_DEPO { get; set; }
    public string DOCUMENT_NO { get; set; }
    public string CUSTOMS_DOCUMENT_ID { get; set; }
    public string TRANSACTION_TYPE_ID { get; set; }
    public string DOCUMENT_SHIPPING_DATE { get; set; }
    public string DOCUMENT_DATE { get; set; }
    public string DOCUMENT_SHIPPING_NO { get; set; }
    public string CUST_ID_PPJK { get; set; }
    public string TERMINAL_ID { get; set; }
  }

  public class CoreorRequest : DataRequest
  {
    public Coreor Request { get; set; }
  }

  public class Coreor
  {
    public string DOCUMENT_NO { get; set; }
    public string BL_NBR { get; set; }
    public string PIN { get; set; }
    public string TERMINAL_ID { get; set; }
  }
}