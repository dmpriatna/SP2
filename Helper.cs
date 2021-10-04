using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

using static System.Text.Encoding;

using static Newtonsoft.Json.JsonConvert;
using static Newtonsoft.Json.Formatting;
using SP2.Data;
using Microsoft.EntityFrameworkCore;
using SP2.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace SP2
{
  public static class Helper
    {
        public static IConfiguration Config { get; set; }

        private static string WebService => Config["WebService"];

        public static GoLogContext Context { get; set; }

        public static XSI Builder => new XSI();
        
        public static XSI FStream(this XSI source, object input)
        {
            source.Instance = source.Instance.Build("fStream", input);
            return source;
        }
        
        public static XSI UserName(this XSI source, string input = "GOLOGS")
        {
            source.Instance = source.Instance.Build("UserName", input);
            return source;
        }
        
        public static XSI Password(this XSI source, string input = "123")
        {
            source.Instance = source.Instance.Build("Password", input);
            return source;
        }
        
        public static XSI DeviceName(this XSI source, string input = "GOLOGS")
        {
            source.Instance = source.Instance.Build("deviceName", input);
            return source;
        }
        
        public static XSI Creator(this XSI source, string input)
        {
            source.Instance = source.Instance.Build("Creator", input);
            return source;
        }

        public static string Build(this string source, string tagName, string input)
        {
            return source + @"<"+ tagName +@" xsi:type=""xsd:string"">"+ input +@"</"+ tagName +@">";
        }

        public static string Build(this string source, string tagName, object content)
        {
            var sContent = SerializeObject(content);
            return source + @"<"+ tagName +@" xsi:type=""xsd:string"">"+ sContent +@"</"+ tagName +@">";
        }

        public static async Task<HttpResponseMessage> PostXmlRequest(string baseUrl, string source)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                    (message, cert, chain, errors) =>
                    {
                        if (cert.Issuer.Equals("CN=localhost"))
                            return true;
                        return errors == System.Net.Security.SslPolicyErrors.None;
                    }
                };

                using (var httpClient = new HttpClient(handler))
                {
                    var httpContent = new StringContent(source, UTF8, "text/xml");
                    // httpContent.Headers.Add("SOAPAction", baseUrl);
                    return await httpClient.PostAsync(baseUrl, httpContent);
                }
            }
            catch (System.Exception se)
            {
                throw se;
            }
        }

        public static XElement XERetrun(this string source, string expression = "//return")
        {
            return XDocument.Parse(source)
                .XPathSelectElement(expression);
        }

        public static string Beautify(this XElement source)
        {
            if (source.IsEmpty)
            return string.Empty;
            try
            {
                return JToken.Parse(source.Value).ToString(Indented);
            }
            catch (System.Exception)
            {
                return source.Value;
            }
        }

        public static async Task<bool> IsActive(this string creator)
        {
            var body = Builder
                .UserName()
                .Password()
                .Creator(creator)
                .Instance;
            var xml = Envelope("MAIN_GetActiveSession", body);

            var response = await PostXmlRequest(WebService, xml);
            if (!response.IsSuccessStatusCode)
                return false;
            var source = await response.Content.ReadAsStringAsync();
            var xE = XDocument.Parse(source)
                .XPathSelectElement("//return");
            var result = DeserializeAnonymousType(xE.Value, new { status = false });
            return result.status;
        }

        public static string EncryptMD5(string source)
        {
            using (var hash = MD5.Create())
            {
                var buffer = UTF8.GetBytes(source);
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
                xmlns:urn=""urn:" + hUrn + @""">
            <soapenv:Header/>
            <soapenv:Body>
                <urn:" + urn + @" soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                " + body + @"
                </urn:" + urn + @">
            </soapenv:Body>
            </soapenv:Envelope>";
        }

        public static async Task<string> GetKoja(this GoLogContext source, string KeyName)
        {
            var entity = await source.Koja
                .Where(w => w.KeyName == KeyName.ToUpper())
                .SingleOrDefaultAsync();
            if (entity == null) return "Key not found";
            return entity.Information;
        }

        public static async Task SetKoja(this GoLogContext source, string KeyName, string Information)
        {
            var now = DateTime.Now;
            var hasData = await source.Koja
                .Where(w => w.KeyName == KeyName.ToUpper())
                .SingleOrDefaultAsync();
            if (hasData == null)
            {
                await source.AddAsync(new Koja
                {
                    CreatedBy = "System",
                    CreatedDate = now,
                    Information = Information,
                    KeyName = KeyName.ToUpper(),
                    ModifiedBy = "System",
                    ModifiedDate = now
                });
                await source.SaveChangesAsync();
            }
            else
            {
                hasData.Information = Information;
                hasData.ModifiedBy = "System";
                hasData.ModifiedDate = now;
            }
        }
   }

    public class XSI
    {
        public string Instance { get; set; }
    }
}