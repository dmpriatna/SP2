using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

using Newtonsoft.Json;

namespace SP2
{
    public static class Helper
    {
        public static async Task<HttpResponseMessage> PostXmlRequest(string baseUrl, string source)
        {
            using (var httpClient = new HttpClient())
            {
                var httpContent = new StringContent(source, Encoding.UTF8, "text/xml");
                httpContent.Headers.Add("SOAPAction", baseUrl);
                return await httpClient.PostAsync(baseUrl, httpContent);
            }
        }

        public static string Beautify(string source)
        {
            var result = XDocument.Parse(source)
                .XPathSelectElement("//return")
                .Value;
            var deserialize = JsonConvert.DeserializeObject(result);
            var pretty = JsonConvert.SerializeObject(deserialize, Formatting.Indented);
            return pretty;
        }

        public static string EncryptMD5(string source)
        {
            using (var hash = MD5.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(source);
                var values = hash.ComputeHash(buffer).Select(s => s.ToString("x2"));
                return string.Concat(values);
            }
        }

        public static string Envelope(string urn, string body)
        {
            var hUrn = urn + "wsdl";
            return @"<?xml version=""1.0"" encoding=""utf-8""?>
            <soapenv:Envelope
                xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:urn=""urn:{" + hUrn + @"}"">
            <soapenv:Header/>
            <soapenv:Body>
                <urn:" + urn + @" soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                " + body + @"
                </urn:" + urn + @">
            </soapenv:Body>
            </soapenv:Envelope>";
        }
    }
}
