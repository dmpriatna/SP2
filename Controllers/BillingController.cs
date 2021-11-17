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
      GPService = _config["GPService"];
      WebService = _config["WebService"];
    }

    string GPService { get; }
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
        var res = DeserializeObject<BaseResponse>(xer.Value);
        if (res.Status)
          await Context.SetKoja("billing_confirmtransaction_true", xer.Value, false);
        else
          await Context.SetKoja("billing_confirmtransaction_false", xer.Value);
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
        if (res.Status)
          await Context.SetKoja("billing_getbilling_true", xer.Value);
        else
          await Context.SetKoja("billing_getbilling_false", xer.Value);
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
        if (res.Status)
          await Context.SetKoja("billing_getproforma_true", xer.Value);
        else
          await Context.SetKoja("billing_getproforma_false", xer.Value);
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
          await Context.SetKoja("billing_getbillingdetail_true", xer.Value);
        else
          await Context.SetKoja("billing_getbillingdetail_false", xer.Value);
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