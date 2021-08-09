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
            var body = @"<UserName xsi:type=""xsd:string"">"+ request.Username +@"</UserName>
                <Password xsi:type=""xsd:string"">"+ request.Password +@"</Password>
                <fStream xsi:type=""xsd:string"">{""PASSWORD"":"""+ Encrypt +
                @""",""USERNAME"":"""+ request.PhoneNumber +@"""}</fStream>
                <deviceName xsi:type=""xsd:string"">GOLOGS</deviceName>";
            var xml = Envelope("AUTH_GetLogin", body);

            var response = await PostXmlRequest(WebService, xml);
            var source = await response.Content.ReadAsStringAsync();
            return Beautify(source);
        }

        [HttpPost]
        public async Task<string> Logout([FromBody] LogOutRequest request)
        {
            var body = @"<UserName xsi:type=""xsd:string"">"+ request.Username +@"</UserName>
                <Password xsi:type=""xsd:string"">"+ request.Password +@"</Password>
                <fStream xsi:type=""xsd:string"">{""SESSIONID"":"""+ request.SessionId +
                @"""}</fStream>
                <deviceName xsi:type=""xsd:string"">GOLOGS</deviceName>";
            var xml = Envelope("AUTH_GetLogOut", body);

            var response = await PostXmlRequest(WebService, xml);
            var source = await response.Content.ReadAsStringAsync();
            return Beautify(source);
        }
    }
}