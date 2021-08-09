using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SP2.Data;
using SP2.Models;
using static SP2.Helper;

namespace SP2.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MasterDataController : ControllerBase
    {
        public MasterDataController(IConfiguration _config, GoLogContext _context)
        {
            WebService = _config.GetSection("WebService").Value;
            Context = _context;
        }

        string WebService { get; }

        GoLogContext Context { get; }

        [HttpPost]
        public async Task<string> ListTerminal([FromBody] DataRequest request)
        {
            string body = @"<UserName xsi:type=""xsd:string"">"+ request.Username +@"</UserName>
                <Password xsi:type=""xsd:string"">"+ request.Password +@"</Password>
                <Creator xsi:type=""xsd:string"">"+ request.Creator +@"</Creator>";
            var xml = Envelope("MAIN_GetTerminal", body);

            var response = await PostXmlRequest(WebService, xml);
            var source = await response.Content.ReadAsStringAsync();

            return Beautify(source);
        }

        [HttpPost]
        public async Task<string> ListTransactionType([FromBody] DocumentRequest request)
        {
            string body = @"<UserName xsi:type=""xsd:string"">"+ request.Username +@"</UserName>
                <Password xsi:type=""xsd:string"">"+ request.Password +@"</Password>
                <Creator xsi:type=""xsd:string"">"+ request.Creator +@"</Creator>
                <fStream xsi:type=""xsd:string"">{""CATEGORY_ID"":"""+ request.CategoryId +
                @""",""TERMINAL_ID"":"""+ request.TerminalId +
                @""",""GROUP_ID"":"""+ request.GroupId +@"""}</fStream>";
            var xml = Envelope("", body);

            var response = await PostXmlRequest(WebService, xml);
            var source = await response.Content.ReadAsStringAsync();

            return Beautify(source);
        }

        [HttpPost]
        public async Task<string> ListDocumentType([FromBody] DocumentRequest request)
        {
            string body = @"<UserName xsi:type=""xsd:string"">"+ request.Username +@"</UserName>
                <Password xsi:type=""xsd:string"">"+ request.Password +@"</Password>
                <Creator xsi:type=""xsd:string"">"+ request.Creator +@"</Creator>
                <fStream xsi:type=""xsd:string"">{""CATEGORY_ID"":"""+ request.CategoryId +
                @""",""TERMINAL_ID"":"""+ request.TerminalId +
                @""",""GROUP_ID"":"""+ request.GroupId +@"""}</fStream>";
            var xml = Envelope("MAIN_GetDocCodeCustoms", body);

            var response = await PostXmlRequest(WebService, xml);
            var source = await response.Content.ReadAsStringAsync();

            return Beautify(source);
        }

        [HttpPost]
        public async Task<string> ActiveSession(string creator)
        {
            string body = @"<UserName xsi:type=""xsd:string"">GOLOGS</UserName>
                <Password xsi:type=""xsd:string"">123</Password>
                <Creator xsi:type=""xsd:string"">"+ creator +@"</Creator>";
            var xml = Envelope("MAIN_GetActiveSession", body);

            var response = await PostXmlRequest(WebService, xml);
            var source = await response.Content.ReadAsStringAsync();

            return Beautify(source);
        }

        [HttpGet]
        public async Task<IActionResult> ListContainer()
        {
            var containers = await Context.Containers.ToListAsync();
            var response = new BaseResponse<List<DOContainer>>
            {
                Data = containers,
                Status = HttpStatusCode.OK
            };
            return Ok(response);
        }
    }
}