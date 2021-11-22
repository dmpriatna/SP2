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
                return JToken.Parse(source.Value.CamelCase()).ToString(Indented);
            }
            catch (System.Exception)
            {
                return source.Value;
            }
        }

        private static string CamelCase(this string source)
        {
          var jo = JObject.Parse(source);
          var props = jo.Properties();
          foreach (var each in props)
          {
            var lower = each.Name.ToLower();
            var chunks = lower.Split('_');
            if (chunks.Length > 1)
            {
              for (int i = 1; i < chunks.Length; i++)
              {
                if (chunks[i].Length > 1)
                  chunks[i] = $"{chunks[i].Substring(0,1).ToUpper()}{chunks[i].Substring(1)}";
              }
              lower = string.Join("", chunks);
            }
            source = source.Replace(each.Name, lower, true, null);
          }
          return source;
        }

        public static async Task<LoginResponse> IsActive(this string creator)
        {
            var body = Builder
                .UserName()
                .Password()
                .Creator(creator)
                .Instance;
            var xml = Envelope("MAIN_GetActiveSession", body);

            var response = await PostXmlRequest(WebService, xml);
            if (!response.IsSuccessStatusCode)
                return new LoginResponse { Message = response.ReasonPhrase };
            var source = await response.Content.ReadAsStringAsync();
            var xer = source.XERetrun();

            if (!xer.IsEmpty)
            {
                var res = DeserializeObject<LoginResponse>(xer.Value);
                if (res.Status)
                {
                    var lastLog = await Context.GetKoja("AUTH_GetLogin_true");
                    res = DeserializeObject<LoginResponse>(lastLog);
                    await Context.SetKoja("MAIN_GetActiveSession_true", xer.Value);
                }
                else
                    await Context.SetKoja("MAIN_GetActiveSession_false", xer.Value);
                return res;
            }

            return new LoginResponse {
                Message = "MAIN_GetActiveSession : response does not have block call return" };
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
            var entity = await source.Set<Koja>()
                .Where(w => w.KeyName == KeyName.ToUpper())
                .SingleOrDefaultAsync();
            if (entity == null) return "Key not found";
            return entity.Information;
        }

        public static async Task SetKoja(this GoLogContext source, string KeyName, string Information, bool replace = true)
        {
            var now = DateTime.Now;
            if (replace)
            {
                var hasData = await source.Set<Koja>()
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
                }
                else
                {
                    hasData.Information = Information;
                    hasData.ModifiedBy = "System";
                    hasData.ModifiedDate = now;
                }
            }
            else
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
            }
            await source.SaveChangesAsync();
        }

        public static string StatusPaid(this string source)
        {
            string result = "\"STATUS_PAID\":[]";
            int st = 0, en = 0;
            
            st = source.IndexOf("STATUS_PAID", StringComparison.CurrentCultureIgnoreCase);
            if (st > 0)
                en = source.IndexOfAny(new [] {']'}, st);
            if (en > 0)
                result = source.Substring(st - 1, en - st + 2);
            
            return result;
        }
        
        public static string Add(this string source, string additional)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(source))
                result = source.Remove(source.Length - 1) + "," + additional + "}";
            return result;
        }

        public static void Changes<TSource, TResult>(this TResult self, TSource source)
        where TResult : class, new()
        {
            if (self == null)
            self = new TResult();

            var properties = source.GetType().GetProperties();
            foreach (var each in properties)
            {
                var en = each.Name;
                var ev = each.GetValue(source);
                self.GetType().GetProperty(en)?.SetValue(self, ev);
            }
        }
    }

    public class XSI
    {
        public string Instance { get; set; }
    }
}