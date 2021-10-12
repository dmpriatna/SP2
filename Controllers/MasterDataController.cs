using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using SP2.Data;
using SP2.Models;
using SP2.Models.Main;

using Swashbuckle.AspNetCore.Annotations;

using static Newtonsoft.Json.JsonConvert;
using static SP2.Helper;

namespace SP2.Controllers
{
  [ApiController]
  [Route("[controller]/[action]")]
  public class MasterDataController : Controller
  {
    public MasterDataController(IConfiguration _config,
        GoLogContext _context)
    {
        Config = _config;
        Context = _context;
        WebService = _config["WebService"];
    }

    string WebService { get; }

    [HttpPost]
    [SwaggerOperation(
        Description = "Info of terminal",
        Summary = "List of terminal"
    )]
    public async Task<IActionResult> Terminal([FromBody]
        DataRequest request)
    {
        var body = Builder
            .UserName()
            .Password()
            .Creator(request.Creator)
            .Instance;
        var xml = Envelope("MAIN_GetTerminal", body);

        var response = await PostXmlRequest(WebService, xml);
        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode);
        var source = await response.Content.ReadAsStringAsync();
        var xer = source.XERetrun();

        if (!xer.IsEmpty)
        {
            var res = DeserializeObject<BaseResponse>(xer.Value);
            if (res.Status)
                await Context.SetKoja("main_getterminal_response_true", xer.Value);
            else
                await Context.SetKoja("main_getterminal_response_false", xer.Value);
        }

        return Ok(xer.Beautify());
    }

    [HttpPost]
    public async Task<IActionResult> TransactionsType([FromBody]
        DocNTransRequest request)
    {
        var body = Builder
            .UserName()
            .Password()
            .Creator(request.Creator)
            .FStream(request.Request)
            .Instance;
        var xml = Envelope("MAIN_GetTransactionsType", body);

        var response = await PostXmlRequest(WebService, xml);
        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode);
        var source = await response.Content.ReadAsStringAsync();
        var xer = source.XERetrun();

        if (!xer.IsEmpty)
        {
            var res = DeserializeObject<BaseResponse>(xer.Value);
            if (res.Status)
                await Context.SetKoja("main_gettransactionstype_response_true", xer.Value);
            else
                await Context.SetKoja("main_gettransactionstype_response_false", xer.Value);
        }

        return Ok(xer.Beautify());
    }

    [HttpPost]
    public async Task<IActionResult> DocCodeCustoms([FromBody]
        DocNTransRequest request)
    {
        var body = Builder
            .UserName()
            .Password()
            .Creator(request.Creator)
            .FStream(request.Request)
            .Instance;
        var xml = Envelope("MAIN_GetDocCodeCustoms", body);

        var response = await PostXmlRequest(WebService, xml);
        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode);
        var source = await response.Content.ReadAsStringAsync();
        var xer = source.XERetrun();

        if (!xer.IsEmpty)
        {
            var res = DeserializeObject<BaseResponse>(xer.Value);
            if (res.Status)
                await Context.SetKoja("main_getdoccodecustoms_response_true", xer.Value);
            else
                await Context.SetKoja("main_getdoccodecustoms_response_false", xer.Value);
        }

        return Ok(xer.Beautify());
    }

    [HttpPost]
    public async Task<IActionResult> DocumentCustomsNGen([FromBody]
        DocNGenRequest request)
    {
        var body = Builder
            .UserName()
            .Password()
            .Creator(request.Creator)
            .FStream(request.Request)
            .Instance;
        var xml = Envelope("MAIN_GetDocumentCustomsNGen", body);

        var response = await PostXmlRequest(WebService, xml);
        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode);
        var source = await response.Content.ReadAsStringAsync();
        var xer = source.XERetrun();

        if (!xer.IsEmpty)
        {
            var res = DeserializeObject<BaseResponse>(xer.Value);
            if (res.Status)
                await Context.SetKoja("main_getdocumentcustomsngen_response_true", xer.Value);
            else
                await Context.SetKoja("main_getdocumentcustomsngen_response_false", xer.Value);
        }

        return Ok(xer.Beautify());
    }

    [HttpPost]
    public async Task<IActionResult> Coreor([FromBody]
        CoreorRequest request)
    {
        var body = Builder
            .UserName()
            .Password()
            .Creator(request.Creator)
            .FStream(request.Request)
            .Instance;
        var xml = Envelope("MAIN_GetCoreor", body);

        var response = await PostXmlRequest(WebService, xml);
        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode);
        var source = await response.Content.ReadAsStringAsync();
        var xer = source.XERetrun();

        if (!xer.IsEmpty)
        {
            var res = DeserializeObject<BaseResponse>(xer.Value);
            if (res.Status)
                await Context.SetKoja("main_getcoreor_response_true", xer.Value);
            else
                await Context.SetKoja("main_getcoreor_response_false", xer.Value);
        }

        return Ok(xer.Beautify());
    }

    [HttpGet]
    private async Task<IActionResult> Container([FromQuery] string creator,
        [FromQuery] string cntrId, [FromQuery] string terminalId,
        [FromQuery] string transactionTypeId)
    {
        var body = Builder
            .UserName()
            .Password()
            .Creator(creator)
            .FStream(new { CNTR_ID = cntrId,
            TRANSACTION_TYPE_ID = transactionTypeId,
            TERMINAL_ID = terminalId })
            .Instance;
        var xml = Envelope("MAIN_GetContainer", body);

        var response = await PostXmlRequest(WebService, xml);
        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode);
        var source = await response.Content.ReadAsStringAsync();

        return Ok(source.XERetrun().Beautify());
    }
  }
}