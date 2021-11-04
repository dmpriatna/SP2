using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                await Context.SetKoja("main_getterminal_true", xer.Value);
            else
                await Context.SetKoja("main_getterminal_false", xer.Value);
        }

        var head = HttpContext.Request.Headers["User-Agent"];
        System.Diagnostics.Debug.WriteLine("\n");
        System.Diagnostics.Debug.WriteLine(head);

        var ip = HttpContext.Connection.RemoteIpAddress;
        System.Diagnostics.Debug.WriteLine("\n");
        System.Diagnostics.Debug.WriteLine(ip);

        var uName = HttpContext.User.Identity.Name;
        System.Diagnostics.Debug.WriteLine("\n");
        System.Diagnostics.Debug.WriteLine(uName);

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
                await Context.SetKoja("main_gettransactionstype_true", xer.Value);
            else
                await Context.SetKoja("main_gettransactionstype_false", xer.Value);
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
                await Context.SetKoja("main_getdoccodecustoms_true", xer.Value);
            else
                await Context.SetKoja("main_getdoccodecustoms_false", xer.Value);
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
                await Context.SetKoja("main_getdocumentcustomsngen_true", xer.Value);
            else
                await Context.SetKoja("main_getdocumentcustomsngen_false", xer.Value);
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
                await Context.SetKoja("main_getcoreor_true", xer.Value);
            else
                await Context.SetKoja("main_getcoreor_false", xer.Value);
        
            var swap = xer.Value.Add(await StatusPaid(request.Creator, request.Request.DocumentNo));
            xer.Value = swap;
        }

        return Ok(xer.Beautify());
    }

    private async Task<string> StatusPaid(string creator, string documentNo)
    {
        var request = new DocNGen
        {
            CustIdPpjk = "371954",
            CustomsDocumentId = "1",
            DocumentNo = documentNo,
            TerminalId = "KOJA",
            TransactionTypeId = "1"
        };
        var body = Builder
            .UserName()
            .Password()
            .Creator(creator)
            .FStream(request)
            .Instance;
        var xml = Envelope("MAIN_GetDocumentCustomsNGen", body);

        var response = await PostXmlRequest(WebService, xml);
        if (!response.IsSuccessStatusCode)
            return string.Empty;
        var source = await response.Content.ReadAsStringAsync();
        var xer = source.XERetrun();

        if (!xer.IsEmpty)
        {
            var res = DeserializeObject<BaseResponse>(xer.Value);
            if (res.Status)
                await Context.SetKoja("Status_Paid_true", xer.Value);
            else
                await Context.SetKoja("Status_Paid_false", xer.Value);
        
            return xer.Value.StatusPaid();
        }

        return "\"STATUS_PAID\":[]";
    }

    [HttpGet]
    private async Task<IActionResult> Container([FromQuery] string creator,
        [FromQuery] string cntrId, [FromQuery] string terminalId = "KOJA",
        [FromQuery] string transactionTypeId = "1")
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

    [HttpGet, Produces("application/json")]
    public async Task<IActionResult> BLNumbers([FromQuery] string npwp, [FromQuery] string doc = "noblbc20")
    {
        try
        {
            string result;
            using (var client = new System.Net.Http.HttpClient())
            {
                var message = await client.GetAsync($"https://esbbcext01.beacukai.go.id:9081/NLEMICROAPI-1.0/webresources/ceisa/{doc}/{npwp}");
                result = await message.Content.ReadAsStringAsync();
            }
            return Ok(result);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    [HttpGet, Produces("application/json")]
    private async Task<IActionResult> DetailDocumentV1(
        [FromQuery] string terminalId,
        [FromQuery] string terminalName,
        [FromQuery] string transactionType,
        [FromQuery] string transactionName,
        [FromQuery] string blnumber,
        [FromQuery] string documentCode,
        [FromQuery] string documentName)
    {
        var result = new { BLDate = "", SPPBNumber = "", SPPBDate = "",
                PIBNumber = "", PIBDate = "", DONumber = "", DODate = "" };
        try
        {
            var query = Context.SP2.AsQueryable();
            if (!string.IsNullOrWhiteSpace(blnumber))
            query = query.Where(w => w.BLNumber.ToLower() == blnumber.ToLower());
            if (!string.IsNullOrWhiteSpace(documentCode))
            query = query.Where(w => w.DocumentCode.ToLower() == documentCode.ToLower());
            if (!string.IsNullOrWhiteSpace(documentName))
            query = query.Where(w => w.DocumentName.ToLower() == documentName.ToLower());
            if (!string.IsNullOrWhiteSpace(terminalId))
            query = query.Where(w => w.TerminalId.ToLower() == terminalId.ToLower());
            if (!string.IsNullOrWhiteSpace(terminalName))
            query = query.Where(w => w.TerminalName.ToLower() == terminalName.ToLower());
            if (!string.IsNullOrWhiteSpace(transactionName))
            query = query.Where(w => w.TransactionName.ToLower() == transactionName.ToLower());
            if (!string.IsNullOrWhiteSpace(transactionType))
            query = query.Where(w => w.TransactionType.ToLower() == transactionType.ToLower());

            var entity = await query.FirstOrDefaultAsync();
            if (entity == null) return Ok(result);
            return Ok(new { entity.BLDate, entity.SPPBNumber, entity.SPPBDate,
                    entity.PIBNumber, entity.PIBDate, entity.DONumber, entity.DODate });
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    [HttpGet, Produces("application/json")]
    private async Task<IActionResult> DetailDocumentV2(
        [FromQuery] string terminalId,
        [FromQuery] string terminalName,
        [FromQuery] string transactionType,
        [FromQuery] string transactionName,
        [FromQuery] string blnumber,
        [FromQuery] string documentCode,
        [FromQuery] string documentName)
    {
        var result = new { BLDate = "", SPPBNumber = "", SPPBDate = "",
                PIBNumber = "", PIBDate = "", DONumber = "", DODate = "" };
        try
        {
            var bl = !string.IsNullOrWhiteSpace(blnumber);
            var doc = !string.IsNullOrWhiteSpace(documentCode) || !string.IsNullOrWhiteSpace(documentName);
            var terminal = !string.IsNullOrWhiteSpace(terminalId) || !string.IsNullOrWhiteSpace(terminalName);
            var trx = !string.IsNullOrWhiteSpace(transactionName) || !string.IsNullOrWhiteSpace(transactionType);

            if (bl && doc && terminal && trx)
            {
                var query = Context.SP2.Where(w => w.BLNumber.ToLower() == blnumber.ToLower());
                if (!string.IsNullOrWhiteSpace(documentCode))
                query = query.Where(w => w.DocumentCode.ToLower() == documentCode.ToLower());
                if (!string.IsNullOrWhiteSpace(documentName))
                query = query.Where(w => w.DocumentName.ToLower() == documentName.ToLower());
                if (!string.IsNullOrWhiteSpace(terminalId))
                query = query.Where(w => w.TerminalId.ToLower() == terminalId.ToLower());
                if (!string.IsNullOrWhiteSpace(terminalName))
                query = query.Where(w => w.TerminalName.ToLower() == terminalName.ToLower());
                if (!string.IsNullOrWhiteSpace(transactionName))
                query = query.Where(w => w.TransactionName.ToLower() == transactionName.ToLower());
                if (!string.IsNullOrWhiteSpace(transactionType))
                query = query.Where(w => w.TransactionType.ToLower() == transactionType.ToLower());
                var entity = await query.FirstOrDefaultAsync();
                if (entity == null) return Ok(result);
                return Ok(new { entity.BLDate, entity.SPPBNumber, entity.SPPBDate,
                    entity.PIBNumber, entity.PIBDate, entity.DONumber, entity.DODate });
            }

            return Ok(result);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    [HttpGet, Produces("application/json"),
    Route("{blNumber}/{blDate}/{transactionType}/{terminalOperator}")]
    public async Task<IActionResult> DocumentDetail(
        [FromRoute] string blNumber,
        [FromRoute] string blDate,
        [FromRoute] string transactionType,
        [FromRoute] string terminalOperator)
    {
        try
        {
            var result = new {
                Status = "Berhasil",
                Message = "Tidak ada data yang dicari"
            };
            blNumber = blNumber.ToLower();
            transactionType = transactionType.ToLower();
            terminalOperator = terminalOperator.ToLower();
            var dateOfBL = System.DateTime.Parse(blDate);
            var entity = await Context.SP2
                .Where(w => w.BLNumber.ToLower() == blNumber &&
                    w.BLDate.Date == dateOfBL.Date &&
                    (w.TransactionName.ToLower() == transactionType
                        || w.TransactionType.ToLower() == transactionType) &&
                    (w.TerminalId.ToLower() == terminalOperator
                        || w.TerminalName.ToLower() == terminalOperator))
                .Select(s => new {
                    DocumentType = s.DocumentName,
                    s.DONumber,
                    s.DODate,
                    s.PIBNumber,
                    s.PIBDate,
                    s.SPPBNumber,
                    s.SPPBDate
                })
                .SingleOrDefaultAsync();
            if (entity == null)
                return Ok(result);

            return Ok(entity);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
  }
}