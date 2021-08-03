using System;
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
    }
}
