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
      GoLogContext _context)
    {
      Config = _config;
      Context = _context;
      WebService = _config["WebService"];
    }

    string WebService { get; }

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
        System.Diagnostics.Debug.WriteLine(xer.Value);
        var res = DeserializeObject<BaseResponse>(xer.Value);
        if (res.Status)
          await Context.SetKoja("billing_confirmtransaction_response_true", xer.Value);
        else
          await Context.SetKoja("billing_confirmtransaction_response_false", xer.Value);
      }

      return Ok(source.XERetrun().Beautify());
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
        System.Diagnostics.Debug.WriteLine(xer.Value);
        var res = DeserializeObject<BaseResponse>(xer.Value);
        if (res.Status)
          await Context.SetKoja("billing_getbilling_response_true", xer.Value);
        else
          await Context.SetKoja("billing_getbilling_response_false", xer.Value);
      }

      return Ok(source.XERetrun().Beautify());
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
        System.Diagnostics.Debug.WriteLine(xer.Value);
        var res = DeserializeObject<BaseResponse>(xer.Value);
        if (res.Status)
          await Context.SetKoja("billing_getproforma_response_true", xer.Value);
        else
          await Context.SetKoja("billing_getproforma_response_false", xer.Value);
      }

      return Ok(source.XERetrun().Beautify());
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
        System.Diagnostics.Debug.WriteLine(xer.Value);
        var res = DeserializeObject<BaseResponse>(xer.Value);
        if (res.Status)
          await Context.SetKoja("billing_getbillingdetail_response_true", xer.Value);
        else
          await Context.SetKoja("billing_getbillingdetail_response_false", xer.Value);
      }

      return Ok(source.XERetrun().Beautify());
    }
  }
}