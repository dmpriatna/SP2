using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

using static System.Text.Encoding;

using static Newtonsoft.Json.JsonConvert;
using static Newtonsoft.Json.Formatting;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json.Linq;

using SP2.Data;
using SP2.Models;

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
      System.Diagnostics.Debug.WriteLine("\n");
      System.Diagnostics.Debug.WriteLine(source);
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
      System.Diagnostics.Debug.WriteLine("\n");
      System.Diagnostics.Debug.WriteLine(source);
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
      return CamelCase(jo, source);
    }

    private static string CamelCase(JObject jo, string source)
    {
      var props = jo.Properties();
      foreach (var each in props)
      {
        if (each.Value.Type == JTokenType.Object)
          source = CamelCase(each.Value as JObject, source);
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

    public static async Task<Guid> SetKoja(this GoLogContext source,
    string KeyName, string Information, bool replace = true,
    string request = null, string response = null)
    {
      System.Diagnostics.Debug.WriteLine("\n");
      System.Diagnostics.Debug.WriteLine(Information);
      Guid createdId = Guid.Empty;
      var now = DateTime.Now;
      if (replace)
      {
        System.Diagnostics.Debug.WriteLine("\n");
        System.Diagnostics.Debug.WriteLine("replace information");
        var hasData = await source.Set<Koja>()
          .Where(w => w.KeyName == KeyName.ToUpper())
          .SingleOrDefaultAsync();

        createdId = await PutKoja(source, hasData, new Koja
        {
          CreatedBy = "System",
          CreatedDate = now,
          Id = Guid.NewGuid(),
          Information = Information,
          KeyName = KeyName.ToUpper(),
          ModifiedBy = "System",
          ModifiedDate = now,
          Request = request,
          Response = response
        });
      }
      else
      {
        System.Diagnostics.Debug.WriteLine("\n");
        System.Diagnostics.Debug.WriteLine("changes information");
        var hasData = await source.Set<Koja>()
          .Where(w => w.KeyName == KeyName.ToUpper()
            && w.Information == Information)
          .SingleOrDefaultAsync();
        var prefix = hasData == null ? "" : "ARC_";

        createdId = await PutKoja(source, hasData, new Koja
        {
          CreatedBy = "System",
          CreatedDate = now,
          Id = Guid.NewGuid(),
          Information = Information,
          KeyName = prefix + KeyName.ToUpper(),
          ModifiedBy = "System",
          ModifiedDate = now,
          Request = request,
          Response = response
        });
      }
      return createdId;
    }

    private static async Task<Guid> PutKoja(GoLogContext context, Koja entity, Koja record)
    {
      Guid result = Guid.Empty;
      if (entity == null)
      {
        await context.AddAsync(record);
        result = record.Id;
      }
      else
      {
        entity.Information = record.Information;
        entity.ModifiedDate = record.ModifiedDate;
        entity.Request = record.Request;
        entity.Response = record.Response;
        result = entity.Id;
      }
      await context.SaveChangesAsync();
      return result;
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
        if (each.PropertyType.Name != "IEnumerable`1" &&
          each.PropertyType.BaseType.Name != "Array")
        self.GetType().GetProperty(en)?.SetValue(self, ev);
      }
    }

    public static string ToQuery(this string[] source, string fromquery)
    {
      var result = fromquery + "=" + string.Join($"&{fromquery}=", source); 
      return result;
    }

    public static async Task<string> TrxNumber(this GoLogContext context,
      string JobNumberFormat = "TR/SP2/{0}-")
    {
      int one = 1;
      string suffix = one.ToString("D6");
      string result = null;
      var now = DateTime.Now;
      var patern = string.Format(JobNumberFormat,
        now.ToString("MM-yyyy/dd"));

      var lastCode = await context.TransactionSet
        .Where(w => w.Delegated && w.CreatedDate.Date == now.Date)
        .OrderBy(ob => ob.CreatedDate)
        .LastOrDefaultAsync();

      if (lastCode == null)
        result = patern + suffix;
      else
      {
        if (string.IsNullOrWhiteSpace(lastCode.TransactionNumber))
          result = patern + suffix;
        else
        {
          var chunk = lastCode.TransactionNumber.Split('-').Last();
          if (int.TryParse(chunk, out int code))
          {
            code++;
            result = patern + code.ToString("D6");
          }
        }
      }
      return result;
    }

    public static bool EqualSafe(this string source, string comparer)
    {
      bool validSource = string.IsNullOrWhiteSpace(source);
      bool validComparer = string.IsNullOrWhiteSpace(comparer);

      if (validSource && validComparer) return true;
      if (validSource || validComparer) return false;

      string _source = source.ToLower();
      string _comparer = comparer.ToLower();

      return _source.Equals(_comparer);
    }

    public static bool ContainSafe(this string source, string comparer)
    {
      bool validSource = string.IsNullOrWhiteSpace(source);
      bool validComparer = string.IsNullOrWhiteSpace(comparer);

      if (validSource && validComparer) return true;
      if (validSource || validComparer) return false;

      string _source = source.ToLower();
      string _comparer = comparer.ToLower();

      return _source.Contains(_comparer);
    }
  }

  public class XSI
  {
    public string Instance { get; set; }
  }

  public class HelperMid
  {
    public HelperMid(RequestDelegate next)
    {
      _next = next;
    }

    private readonly RequestDelegate _next;

    public async Task Invoke(HttpContext context)
    {
      var currentBody = context.Response.Body;

      await using var memoryStream = new MemoryStream();
      context.Response.Body = memoryStream;
      ContentResult error = null;
      try
      {
        await _next(context);
      }
      // catch (ApiException apiException)
      // {
      //     logger.LogException(apiException, apiException.EventId);
      //     context.Response.StatusCode = (int)apiException.StatusCode;
      //     error = new ErrorDetail
      //     {
      //         ErrorCode = apiException.ErrorCode,
      //         ErrorDescription = apiException.Message,
      //         StatusCode = apiException.StatusCode
      //     };
      // }
      catch (Exception se)
      {
        context.Response.StatusCode = 500;
        error = new ContentResult
        {
          Content = Newtonsoft.Json.JsonConvert.SerializeObject(new { Message = se.Message }),
          ContentType = "application/json",
          StatusCode = 422
        };
      }

      context.Response.Body = currentBody;
      memoryStream.Seek(0, SeekOrigin.Begin);

      var readToEnd = await new StreamReader(memoryStream).ReadToEndAsync();
      if (string.Compare(context.Response.ContentType, "application/pdf", StringComparison.InvariantCultureIgnoreCase) == 0 || string.Compare(context.Response.ContentType, "image/png", StringComparison.InvariantCultureIgnoreCase) == 0)
      {
        var buffer = memoryStream.ToArray();
        await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
        return;
      }
      if (context.Response.StatusCode == 200 && context.Response.ContentType != null && !context.Response.ContentType.Contains("application/json"))
      {
        await context.Response.WriteAsync(readToEnd);
        return;
      }
      if (context.Response.StatusCode == 403 || context.Response.StatusCode == 401)
      {
        context.Response.ContentType = "application/json; charset=utf-8";
        var serializeObject = SerializeObject(new ContentResult
        {
          Content = Newtonsoft.Json.JsonConvert.SerializeObject(new { Message = "You dont have any permission to access" }),
          ContentType = "application/json",
          StatusCode = context.Response.StatusCode
        });
        await context.Response.WriteAsync(serializeObject);
      }

      if (error != null)
      {
        context.Response.ContentType = "application/json; charset=utf-8";
        await context.Response.WriteAsync(SerializeObject(error));
      }
      else
      {
        await context.Response.WriteAsync(readToEnd);
      }
    }
  }
}