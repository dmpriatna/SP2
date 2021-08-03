using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using SP2.Models;
using static SP2.Helper;

namespace SP2.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Authentication
    {
        public Authentication(IConfiguration _config)
        {
            WebService = _config.GetSection("WebService").Value;
        }

        string WebService { get; }

        [HttpPost]
        public async Task<string> Login([FromBody] LogInRequest request)
        {
            var Encrypt = EncryptMD5(request.Password);
            string body = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <soapenv:Envelope
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:urn=""urn:AUTH_GetLoginwsdl"">
            <soapenv:Header/>
            <soapenv:Body>
                <urn:AUTH_GetLogin soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                    <UserName xsi:type=""xsd:string"">"+ request.Username +@"</UserName>
                    <Password xsi:type=""xsd:string"">"+ request.Password +@"</Password>
                    <fStream xsi:type=""xsd:string"">{""PASSWORD"":"""+ Encrypt +@""",""USERNAME"":"""+ request.PhoneNumber +@"""}</fStream>
                    <deviceName xsi:type=""xsd:string"">GOLOGS</deviceName>
                </urn:AUTH_GetLogin>
            </soapenv:Body>
            </soapenv:Envelope>";

            var response = await PostXmlRequest(WebService, body);
            var source = await response.Content.ReadAsStringAsync();
            return Beautify(source);
        }

        [HttpPost]
        public async Task<string> Logout([FromBody] LogOutRequest request)
        {
            string body = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <soapenv:Envelope
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:urn=""urn:AUTH_GetLogOutwsdl"">
            <soapenv:Header/>
            <soapenv:Body>
                <urn:AUTH_GetLogOut soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                    <UserName xsi:type=""xsd:string"">"+ request.Username +@"</UserName>
                    <Password xsi:type=""xsd:string"">"+ request.Password +@"</Password>
                    <fStream xsi:type=""xsd:string"">{""SESSIONID"":"""+ request.SessionId +@"""}</fStream>
                    <deviceName xsi:type=""xsd:string"">GOLOGS</deviceName>
                </urn:AUTH_GetLogOut>
            </soapenv:Body>
            </soapenv:Envelope>";

            var response = await PostXmlRequest(WebService, body);
            var source = await response.Content.ReadAsStringAsync();
            return Beautify(source);
        }
    }
}