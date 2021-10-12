using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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
    [JsonProperty("CATEGORY_ID")] public string CategoryId { get; set; } = "I";
    [JsonProperty("TERMINAL_ID")] public string TerminalId { get; set; } = "KOJA";
    [JsonProperty("GROUP_ID")] public string GroupId { get; set; } = "11";
  }

  public class DocNGenRequest : DataRequest
  {
    public DocNGen Request { get; set; }
  }

  public class DocNGen
  {
    [JsonProperty("NPWP_DEPO")] public string NpwpDepo { get; set; }
    [JsonProperty("DOCUMENT_NO")] public string DocumentNo { get; set; }
    [JsonProperty("CUSTOMS_DOCUMENT_ID")] public string CustomsDocumentId { get; set; }
    [JsonProperty("TRANSACTION_TYPE_ID")] public string TransactionTypeId { get; set; }
    [JsonProperty("DOCUMENT_SHIPPING_DATE")] public string DocumentShippingDate { get; set; }
    [JsonProperty("DOCUMENT_DATE")] public string DocumentDate { get; set; }
    [JsonProperty("DOCUMENT_SHIPPING_NO")] public string DocumentShippingNo { get; set; }
    [JsonProperty("CUST_ID_PPJK")] public string CustIdPpjk { get; set; }
    [JsonProperty("TERMINAL_ID")] public string TerminalId { get; set; }
  }

  public class CoreorRequest : DataRequest
  {
    public Coreor Request { get; set; }
  }

  public class Coreor
  {
    [JsonProperty("DOCUMENT_NO")] public string DocumentNo { get; set; }
    [JsonProperty("BL_NBR")] public string BlNbr { get; set; }
    [JsonProperty("PIN")] public string Pin { get; set; }
    [JsonProperty("TERMINAL_ID")] public string TerminalId { get; set; }
  }
}