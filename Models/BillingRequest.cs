using Newtonsoft.Json;
using SP2.Models.Main;

namespace SP2.Models.Billing
{
  public class ConfirmTransactionRequest : DataRequest
  {
    public ConfirmTransaction Request { get; set; }
  }

  public class ConfirmTransaction
  {
    [JsonProperty("CUST_ID")] public string CustId { get; set; }
    [JsonProperty("PM_ID")] public string PmId { get; set; }
    [JsonProperty("EMAIL_REQ")] public string EmailReq { get; set; }
    [JsonProperty("PHONE_REQ")] public string PhoneReq { get; set; }
    [JsonProperty("CUST_ID_REQ")] public string CustIdReq { get; set; }
    [JsonProperty("CUST_ID_PPJK")] public string CustIdPpjk { get; set; }
    [JsonProperty("TRANSACTIONS_TYPE_ID")] public string TransactionsTypeId { get; set; }
    [JsonProperty("CUSTOMS_DOCUMENT_ID")] public string CustomsDocumentId { get; set; }
    [JsonProperty("DOCUMENT_NO")] public string DocumentNo { get; set; }
    [JsonProperty("DOCUMENT_DATE")] public string DocumentDate { get; set; }
    [JsonProperty("DOCUMENT_SHIPPING_NO")] public string DocumentShippingNo { get; set; }
    [JsonProperty("DOCUMENT_SHIPPING_DATE")] public string DocumentShippingDate { get; set; }
    [JsonProperty("NO_BL_AWB")] public string NoBlAwb { get; set; }
    [JsonProperty("PAID_THRU")] public string PaidThru { get; set; }
    [JsonProperty("VOYAGE_NO")] public string VoyageNo { get; set; }
    [JsonProperty("VESSEL_ID")] public string VesselId { get; set; }

    [JsonProperty("NO_CONT")] public string[] NoCont { get; set; }
    [JsonProperty("OWNER")] public string[] Owner { get; set; }
    [JsonProperty("ISO_CODE")] public string[] IsoCode { get; set; }
    [JsonProperty("WEIGHT")] public string[] Weight { get; set; }
    [JsonProperty("POL")] public string[] Pol { get; set; }
    [JsonProperty("POD")] public string[] Pod { get; set; }
    [JsonProperty("FD")] public string[] Fd { get; set; }

    [JsonProperty("OLD_COMPANY_CODE")] public string OldCompanyCode { get; set; }
    [JsonProperty("OLD_VOYAGE_NO")] public string OldVoyageNo { get; set; }
    [JsonProperty("COMPANY_CODE")] public string CompanyCode { get; set; }
    [JsonProperty("OLD_VESSEL_ID")] public string OldVesselId { get; set; }
    [JsonProperty("OLD_INVOICE_NO")] public string OldInvoiceNo { get; set; }
    [JsonProperty("TGL_NHI")] public string TglNhi { get; set; }
    [JsonProperty("TGL_BK_SEGEL_NHI")] public string TglBkSegelNhi { get; set; }
    [JsonProperty("QUEUE_COUNTER_ID")] public string QueueCounterId { get; set; }

    [JsonProperty("CERTIFICATED_ID")] public string[] CertificatedId { get; set; }
    [JsonProperty("OVER_RIGHT")] public string[] OverRight { get; set; }
    [JsonProperty("START_PLUG")] public string[] StartPlug { get; set; }
    [JsonProperty("OVER_LEFT")] public string[] OverLeft { get; set; }
    [JsonProperty("CERTIFICATED_PIC")] public string[] CertificatedPic { get; set; }
    [JsonProperty("IMO_CODE")] public string[] ImoCode { get; set; }
    [JsonProperty("UN_NUMBER")] public string[] UnNumber { get; set; }
    [JsonProperty("CUST_SERTIFICATED")] public string[] CustSertificated { get; set; }
    [JsonProperty("STOP_PLUG")] public string[] StopPlug { get; set; }
    [JsonProperty("REFEER_TEMPERATURE")] public string[] RefeerTemperature { get; set; }
    [JsonProperty("VOLTAGE_PLUG")] public string[] VoltagePlug { get; set; }
    [JsonProperty("OLD_POD")] public string[] OldPod { get; set; }
    [JsonProperty("WEIGHT_VGM")] public string[] WeightVgm { get; set; }
    [JsonProperty("OLD_NO_CONT")] public string[] OldNoCont { get; set; }
    [JsonProperty("NPWP_SERTIFICATED")] public string[] NpwpSetificated { get; set; }
    [JsonProperty("OLD_FD")] public string[] OldFd { get; set; }
    [JsonProperty("OVER_FRONT")] public string[] OverFront { get; set; }
    [JsonProperty("OVER_BACK")] public string[] OverBack { get; set; }
    [JsonProperty("OVER_HEIGHT")] public string[] OverHeight { get; set; }
  }

  public class BillingRequest : DataRequest
  {
    public Billing Request { get; set; }
  }

  public class Billing
  {
    [JsonProperty("TRANSACTION_ID")] public string TransactionId { get; set; }
  }

  public class ProformaRequest : DataRequest
  {
    public Proforma Request { get; set; }
  }

  public class Proforma
  {
    [JsonProperty("TIME_SECOND")] public string TimeSecond { get; set; }
    [JsonProperty("TRANSACTION_ID")] public string TransactionId { get; set; }
  }

  public class BillingDetailRequest : DataRequest
  {
    public BillingDetail Request { get; set; }
  }

  public class BillingDetail
  {
    [JsonProperty("TRANSACTIONS_TYPE_ID")] public string TransactionsTypeId { get; set; }
    [JsonProperty("PROFORMA_INVOICE_NO")] public string ProformaInvoiceNo { get; set; }
    [JsonProperty("INVOICE_NO")] public string InvoiceNo { get; set; }
  }

  class WebBaseConfirmTransactionRequest
  {
    public string TRANSACTIONS_TYPE_ID { get; set; }
    public string CUST_ID_PPJK { get; set; }
    public string CUST_ID { get; set; }
    public string CUSTOMS_DOCUMENT_ID { get; set; }
    public string DOCUMENT_NO { get; set; }
    public string DOCUMENT_DATE { get; set; }
    public string DOCUMENT_SHIPPING_NO { get; set; }
    public string DOCUMENT_SHIPPING_DATE { get; set; }
    public string PAID_THRU { get; set; }
    public string VESSEL_ID { get; set; }
    public string VOYAGE_NO { get; set; }
    public string NO_CONT { get; set; }
    public string START_PLUG { get; set; }
    public string STOP_PLUG { get; set; }
    public string OLD_INVOICE_NO { get; set; }
    public string COMPANY_CODE { get; set; }
    public string REFEER_TEMPERATURE { get; set; }
    public string VOLTAGE_PLUG { get; set; }
    public string OVER_HEIGHT { get; set; }
    public string OVER_FRONT { get; set; }
    public string OVER_RIGHT { get; set; }
    public string OVER_LEFT { get; set; }
    public string OVER_BACK { get; set; }
    public string IMO_CODE { get; set; }
    public string WEIGHT { get; set; }
    public string OWNER { get; set; }
    public string POL { get; set; }
    public string POD { get; set; }
    public string FD { get; set; }
    public string OLD_COMPANY_CODE { get; set; }
    public string OLD_VESSEL_ID { get; set; }
    public string OLD_VOYAGE_NO { get; set; }
    public string OLD_POD { get; set; }
    public string OLD_FD { get; set; }
    public string OLD_NO_CONT { get; set; }
    public string CERTIFICATED_ID { get; set; }
    public string CERTIFICATED_PIC { get; set; }
    public string CUST_SERTIFICATED { get; set; }
    public string NPWP_SERTIFICATED { get; set; }
    public string WEIGHT_VGM { get; set; }
  }
}