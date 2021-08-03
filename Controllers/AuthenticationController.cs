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

        [NonAction]
        public string Extract()
        {
            var source = @"<?xml version=""1.0"" encoding=""ISO-8859-1""?><SOAP-ENV:Envelope SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""><SOAP-ENV:Body><ns1:AUTH_GetLoginResponse xmlns:ns1=""urn:AUTH_GetLoginwsdl""><return xsi:type=""xsd:string"">{&quot;OWNER_CODE&quot;:[&quot;KOJA&quot;,&quot;KOJA&quot;,&quot;KOJA&quot;,&quot;KOJA&quot;,&quot;KOJA&quot;,&quot;KOJA&quot;,&quot;KOJA&quot;],&quot;CUST_ID_2&quot;:null,&quot;GROUPID&quot;:&quot;11&quot;,&quot;USERNAME&quot;:&quot;081230943248&quot;,&quot;SESSIONID&quot;:&quot;1528200&quot;,&quot;CUST_ID&quot;:&quot;1&quot;,&quot;CUST_NAME&quot;:&quot;KSO TPKKOJA&quot;,&quot;NPWP&quot;:&quot;01.804.534.4-042.000&quot;,&quot;CUST_TYPE_ID&quot;:&quot;TPS&quot;,&quot;GROUP_TYPE&quot;:&quot;USR&quot;,&quot;GROUP_NAME&quot;:&quot;USER&quot;,&quot;EXPIRED_DATE&quot;:&quot;2021-07-10 00:00:00&quot;,&quot;ACTIVE_USER&quot;:null,&quot;PASSWORD&quot;:&quot;202cb962ac59075b964b07152d234b70&quot;,&quot;EMAIL&quot;:null,&quot;PHONE&quot;:&quot;081230943248&quot;,&quot;USER_TYPE&quot;:&quot;2&quot;,&quot;NIK&quot;:null,&quot;EXPIRED_USER&quot;:-23,&quot;EXPIRED_STATUS&quot;:&quot;VALID&quot;,&quot;STATUS&quot;:&quot;TRUE&quot;,&quot;MENU_ID&quot;:[&quot;1&quot;,&quot;2&quot;,&quot;3&quot;],&quot;MENU_ID_PARENT&quot;:[null,null,&quot;2&quot;],&quot;TITLE&quot;:[&quot;Dashboard&quot;,&quot;Transactions&quot;,&quot;Container Import&quot;],&quot;TYPE&quot;:[&quot;F&quot;,&quot;F&quot;,&quot;M&quot;],&quot;URL&quot;:[&quot;#&quot;,&quot;#&quot;,&quot;BillingImportForm&quot;],&quot;PARAMETER&quot;:[null,null,&quot;{\&quot;CATEGORY_ID\&quot;:\&quot;I\&quot;}&quot;],&quot;DATA_SESSIONS&quot;:{&quot;STATUS&quot;:&quot;FALSE&quot;,&quot;MESSAGE&quot;:&quot;Data tidak ditemukan.&quot;},&quot;CATEGORY_ID&quot;:[&quot;M&quot;,&quot;IE&quot;,&quot;I&quot;,&quot;E&quot;],&quot;CATEGORY_NAME&quot;:[&quot;MANUAL&quot;,&quot;EXPORT IMPORT&quot;,&quot;IMPORT&quot;,&quot;EXPORT&quot;],&quot;TERMINAL_ID&quot;:[&quot;JICT&quot;,&quot;KOJA&quot;],&quot;TERMINAL_NAME&quot;:[&quot;JAKARTA INTERNATIONAL CONTAINER TERMINAL&quot;,&quot;KSO TPK KOJA&quot;]}</return></ns1:AUTH_GetLoginResponse></SOAP-ENV:Body></SOAP-ENV:Envelope>";
            
            return Beautify(source);
        }
    }
}