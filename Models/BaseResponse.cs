using System.Net;
using Newtonsoft.Json;

namespace SP2.Models
{
  public class BaseResponse<T>
  {
    public HttpStatusCode Status { get; set; }
    private string _message;
    public string Message
    {
      get
      {
        return Status == HttpStatusCode.OK ? "Success" : _message;
      }
      set
      {
        _message = value;
      }
    }
    public T Data { get; set; }
  }

  public class BaseResponse
  {
    public bool Status { get; set; }
    public string Message { get; set; }
  }

  public class LoginResponse : BaseResponse
  {
    [JsonProperty("CATEGORY_ID")] public string[] CategoryId { get; set; }
    [JsonProperty("CATEGORY_NAME")] public string[] CategoryName { get; set; }
    [JsonProperty("CUST_ID")] public string CustId { get; set; }
    [JsonProperty("CUST_NAME")] public string CustName { get; set; }
    [JsonProperty("GROUPID")] public string GroupId { get; set; }
    [JsonProperty("SESSIONID")] public string SessionId { get; set; }
    [JsonProperty("TERMINAL_ID")] public string[] TerminalId { get; set; }
    [JsonProperty("TERMINAL_NAME")] public string[] TerminalName { get; set; }
  }

  public class BillingResponse : BaseResponse
  {
    [JsonProperty("PROFORMA_INVOICE_NO")] public string ProformaInvoiceNo { get; set; }
    [JsonProperty("DETAIL_BILLING")] public BDetail DetailBilling { get; set; }
  }

  public class BDetail : BaseResponse
  {
    [JsonProperty("TRX_EMAIL")] public string TrxEmail { get; set; }
    [JsonProperty("TRX_NAME")] public string TrxName { get; set; }
    [JsonProperty("TRX_ID")] public string TrxId { get; set; }
    [JsonProperty("NO_CONT")] public string[] NoCont { get; set; }
  }
}