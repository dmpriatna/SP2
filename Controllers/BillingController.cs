using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using SP2.Data;
using SP2.Models;
using SP2.Models.Billing;

using static Newtonsoft.Json.JsonConvert;
using static SP2.Helper;

namespace SP2.Controllers
{
  [ApiController]
  [Route("[controller]/[action]")]
  public class BillingController : Controller
  {
    public BillingController(IConfiguration _config,
      GoLogContext _context,
      IService _service)
    {
      Config = _config;
      Context = _context;
      GPService = _config["GPService"];
      WebService = _config["WebService"];
      Service = _service;
    }

    string GPService { get; }
    string WebService { get; }
    IService Service { get; }

    [HttpPost]
    public async Task<IActionResult> ConfirmTransaction([FromBody]
      ConfirmTransactionRequest request)
    {
      var body = Builder
        .UserName()
        .Password()
        .Creator(request.Creator)
        .FStream(request.Request)
        .Instance;
      var xml = Envelope("BILLING_ConfirmTransaction", body);

      var response = await PostXmlRequest(WebService, xml);
      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode);
      var source = await response.Content.ReadAsStringAsync();
      var xer = source.XERetrun();

      if (!xer.IsEmpty)
      {
        var res = DeserializeObject<BaseResponse>(xer.Value);
        if (res.Status)
        {
          var ci = await Context.SetKoja("billing_confirmtransaction_true", xer.Value, false, xml, source);
          await Service.PutTransaction(new TransactionDto
          {
            CompanyId = Guid.Parse("831ac973-af04-4406-8a90-c06dd025989d"),
            RowStatus = true,
            TransactionNumber = ci.ToString(),
            TransactionTypeId = Guid.Parse("b02ea995-662b-4056-8a0c-0e69a1a98c35")
          });
        }
        else
          await Context.SetKoja("billing_confirmtransaction_false", xer.Value, false, xml, source);
      }

      return Ok(xer.Beautify());
    }

    [HttpPost]
    public async Task<IActionResult> Billing([FromBody]
      BillingRequest request)
    {
      var body = Builder
        .UserName()
        .Password()
        .Creator(request.Creator)
        .FStream(request.Request)
        .Instance;
      var xml = Envelope("BILLING_GetBilling", body);

      var response = await PostXmlRequest(WebService, xml);
      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode);
      var source = await response.Content.ReadAsStringAsync();
      var xer = source.XERetrun();

      if (!xer.IsEmpty)
      {
        var res = DeserializeObject<BaseResponse>(xer.Value);
        await Context.SetKoja($"billing_getbilling_{request.Request.TransactionId}_{res.Status.ToString()}", xer.Value, false, xml, source);
      }

      return Ok(xer.Beautify());
    }

    [HttpPost]
    public async Task<IActionResult> Proforma([FromBody]
      ProformaRequest request)
    {
      var body = Builder
        .UserName()
        .Password()
        .Creator(request.Creator)
        .FStream(request.Request)
        .Instance;
      var xml = Envelope("BILLING_GetProforma", body);

      var response = await PostXmlRequest(WebService, xml);
      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode);
      var source = await response.Content.ReadAsStringAsync();
      var xer = source.XERetrun();

      if (!xer.IsEmpty)
      {
        var res = DeserializeObject<BaseResponse>(xer.Value);
        await Context.SetKoja($"billing_getproforma_{request.Request.TransactionId}_{res.Status.ToString()}", xer.Value, false, xml, source);
      }

      return Ok(xer.Beautify());
    }

    [HttpPost]
    public async Task<IActionResult> BillingDetail([FromBody]
      BillingDetailRequest request)
    {
      var body = Builder
        .UserName()
        .Password()
        .Creator(request.Creator)
        .FStream(request.Request)
        .Instance;
      var xml = Envelope("BILLING_GetBillingDetail", body);

      var response = await PostXmlRequest(WebService, xml);
      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode);
      var source = await response.Content.ReadAsStringAsync();
      var xer = source.XERetrun();

      if (!xer.IsEmpty)
      {
        var res = DeserializeObject<BaseResponse>(xer.Value);
        if (res.Status)
        {
          var bdnumber = string.IsNullOrWhiteSpace(request.Request.ProformaInvoiceNo) ?
            request.Request.InvoiceNo : request.Request.ProformaInvoiceNo;
          await Context.SetKoja($"billing_getbillingdetail_{request.Request.ProformaInvoiceNo}_true", xer.Value, false, xml, source);
          var br = DeserializeObject<BillingResponse>(xer.Value);
          if (br.DetailBilling.Status)
          await Service.SendMail(new EmailDto {
            CustEmail = br.DetailBilling.TrxEmail,
            CustName = br.DetailBilling.TrxName,
            EmailCC = new[] { br.DetailBilling.TrxEmail },
            GpUrl = $"http://13.213.73.45:3500/Billing/GatePass?proforma={br.ProformaInvoiceNo}&{br.DetailBilling.NoCont.ToQuery("container")}&filename=GP01",
            TransNum = br.DetailBilling.TrxId
          });
        }
        else
          await Context.SetKoja($"billing_getbillingdetail_{request.Request.ProformaInvoiceNo}_false", xer.Value, false, xml, source);
      }

      return Ok(xer.Beautify());
    }

    [HttpGet]
    public async Task<IActionResult> GatePass(
      [FromQuery] string proforma,
      [FromQuery] string[] container,
      [FromQuery] string filename
    )
    {
      try
      {
        using (var client = new System.Net.Http.HttpClient())
        {
          var ID = SerializeObject(new {
            CONTAINER = container,
            PROFORMA = proforma,
            ACTION = "V"
          });
          var requestUri = $"{GPService}?ID={ID}&FILENAME={filename}&METHOD=FD";
          var response = await client.GetAsync(requestUri);
          var stream = await response.Content.ReadAsStreamAsync();
          return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, $"{filename}.pdf");
        }
      }
      catch (System.Exception)
      {
        throw;
      }
    }
  }
}