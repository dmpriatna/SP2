using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using SP2.Models;
using static SP2.Helper;

namespace SP2.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MasterDataController : ControllerBase
    {
        public MasterDataController(IConfiguration _config)
        {
            WebService = _config.GetSection("WebService").Value;
        }

        string WebService { get; }

        [HttpPost]
        public async Task<string> ListTerminal([FromBody] DataRequest request)
        {
            string body = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <soapenv:Envelope
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:urn=""urn:{MAIN_GetTerminalwsdl}"">
            <soapenv:Header/>
            <soapenv:Body>
                <urn:MAIN_GetTerminal soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                    <UserName xsi:type=""xsd:string"">"+ request.Username +@"</UserName>
                    <Password xsi:type=""xsd:string"">"+ request.Password +@"</Password>
                    <Creator xsi:type=""xsd:string"">"+ request.Creator +@"</Creator>
                </urn:MAIN_GetTerminal>
            </soapenv:Body>
            </soapenv:Envelope>";

            var response = await PostXmlRequest(WebService, body);
            var source = await response.Content.ReadAsStringAsync();

            return Beautify(source);
        }

        [HttpPost]
        public async Task<string> ListTransactionType([FromBody] DocumentRequest request)
        {
            string body = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <soapenv:Envelope
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:urn=""urn:{MAIN_GetTransactionsTypewsdl}"">
            <soapenv:Header/>
            <soapenv:Body>
                <urn:MAIN_GetTransactionsType soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                    <UserName xsi:type=""xsd:string"">"+ request.Username +@"</UserName>
                    <Password xsi:type=""xsd:string"">"+ request.Password +@"</Password>
                    <Creator xsi:type=""xsd:string"">"+ request.Creator +@"</Creator>
                    <fStream xsi:type=""xsd:string"">{""CATEGORY_ID"":"""+ request.CategoryId +@""",""TERMINAL_ID"":"""+ request.TerminalId +@""",""GROUP_ID"":"""+ request.GroupId +@"""}</fStream>
                </urn:MAIN_GetTransactionsType>
            </soapenv:Body>
            </soapenv:Envelope>";

            var response = await PostXmlRequest(WebService, body);
            var source = await response.Content.ReadAsStringAsync();

            return Beautify(source);
        }

        [HttpPost]
        public async Task<string> ListDocumentType([FromBody] DocumentRequest request)
        {
            string body = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <soapenv:Envelope
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:urn=""urn:{MAIN_GetDocCodeCustomswsdl}"">
            <soapenv:Header/>
            <soapenv:Body>
                <urn:MAIN_GetDocCodeCustoms soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                    <UserName xsi:type=""xsd:string"">"+ request.Username +@"</UserName>
                    <Password xsi:type=""xsd:string"">"+ request.Password +@"</Password>
                    <Creator xsi:type=""xsd:string"">"+ request.Creator +@"</Creator>
                    <fStream xsi:type=""xsd:string"">{""CATEGORY_ID"":"""+ request.CategoryId +@""",""TERMINAL_ID"":"""+ request.TerminalId +@""",""GROUP_ID"":"""+ request.GroupId +@"""}</fStream>
                </urn:MAIN_GetDocCodeCustoms>
            </soapenv:Body>
            </soapenv:Envelope>";

            var response = await PostXmlRequest(WebService, body);
            var source = await response.Content.ReadAsStringAsync();

            return Beautify(source);
        }
    }
}