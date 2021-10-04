using SP2.Models.Main;

namespace SP2.Models.Billing
{
  public class ConfirmTransactionRequest : DataRequest
  {
    public ConfirmTransaction Request { get; set; }
  }

  public class ConfirmTransaction
  {
    public string[] CERTIFICATED_ID { get; set; }
    public string[] ISO_CODE { get; set; }
    public string[] OVER_RIGHT { get; set; }
    public string[] START_PLUG { get; set; }
    public string[] OVER_LEFT { get; set; }
    public string[] OWNER { get; set; }
    public string[] WEIGHT { get; set; }
    public string[] CERTIFICATED_PIC { get; set; }
    public string[] IMO_CODE { get; set; }
    public string[] UN_NUMBER { get; set; }
    public string[] POD { get; set; }
    public string[] CUST_SERTIFICATED { get; set; }
    public string[] STOP_PLUG { get; set; }
    public string[] POL { get; set; }
    public string[] FD { get; set; }
    public string[] NO_CONT { get; set; }
    public string[] REFEER_TEMPERATURE { get; set; }
    public string[] VOLTAGE_PLUG { get; set; }
    public string[] OLD_POD { get; set; }
    public string[] WEIGHT_VGM { get; set; }
    public string[] OLD_NO_CONT { get; set; }
    public string[] NPWP_SERTIFICATED { get; set; }
    public string[] OLD_FD { get; set; }
    public string[] OVER_FRONT { get; set; }
    public string[] OVER_BACK { get; set; }
    public string[] OVER_HEIGHT { get; set; }

    
    public string OLD_COMPANY_CODE { get; set; }
    public string CUST_ID { get; set; }
    public string TRANSACTIONS_TYPE_ID { get; set; }
    public string DOCUMENT_SHIPPING_NO { get; set; }
    public string OLD_VOYAGE_NO { get; set; }
    public string PM_ID { get; set; }
    public string DOCUMENT_SHIPPING_DATE { get; set; }
    public string VOYAGE_NO { get; set; }
    public string COMPANY_CODE { get; set; }
    public string OLD_VESSEL_ID { get; set; }
    public string VESSEL_ID { get; set; }
    public string DOCUMENT_DATE { get; set; }
    public string CUST_ID_PPJK { get; set; }
    public string OLD_INVOICE_NO { get; set; }
    public string EMAIL_REQ { get; set; }
    public string TGL_NHI { get; set; }
    public string CUSTOMS_DOCUMENT_ID { get; set; }
    public string NO_BL_AWB { get; set; }
    public string PHONE_REQ { get; set; }
    public string DOCUMENT_NO { get; set; }
    public string PAID_THRU { get; set; }
    public string TGL_BK_SEGEL_NHI { get; set; }
    public string QUEUE_COUNTER_ID { get; set; }
    public string CUST_ID_REQ { get; set; }
  }

  public class BillingRequest : DataRequest
  {
    public Billing Request { get; set; }
  }

  public class Billing
  {
    public string TRANSACTION_ID { get; set; }
  }

  public class ProformaRequest : DataRequest
  {
    public Proforma Request { get; set; }
  }

  public class Proforma
  {
    public string TIME_SECOND { get; set; }
    public string TRANSACTION_ID { get; set; }
  }

  public class BillingDetailRequest : DataRequest
  {
    public BillingDetail Request { get; set; }
  }

  public class BillingDetail
  {
    public string TRANSACTIONS_TYPE_ID { get; set; }
    public string PROFORMA_INVOICE_NO { get; set; }
    public string INVOICE_NO { get; set; }
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