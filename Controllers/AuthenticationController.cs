using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using SP2.Data;
using SP2.Models;

using static Newtonsoft.Json.JsonConvert;
using static SP2.Helper;

namespace SP2.Controllers
{
  [ApiController]
    [Route("[controller]/[action]")]
    public class Authentication : Controller
    {
        public Authentication(IConfiguration _config, GoLogContext _context)
        {
            Config = _config;
            Context = _context;
            WebService = _config["WebService"];
        }

        string WebService { get; }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]
            LogInRequest request)
        {
            var SessionId = await Context.GetKoja("SESSIONID");
            if (SessionId != "Key not found")
            {
                var IsActive = await SessionId.IsActive();
                if (IsActive)
                    return Ok(SerializeObject(new { SESSIONID = SessionId, STATUS = "TRUE" }));
            }

            var Encrypt = EncryptMD5(request.Password);
            var body = Builder
                .UserName(request.Username)
                .Password(request.Password)
                .DeviceName()
                .FStream(new { PASSWORD = Encrypt, USERNAME = request.PhoneNumber })
                .Instance;
            var xml = Envelope("AUTH_GetLogin", body);

            var response = await PostXmlRequest(WebService, xml);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);
            var source = await response.Content.ReadAsStringAsync();
            var xer = source.XERetrun();

            if (!xer.IsEmpty)
            {
                System.Diagnostics.Debug.WriteLine(xer.Value);
                var res = DeserializeObject<LoginResponse>(xer.Value);
                if (res.Status)
                {
                    await Context.SetKoja("login_response_true", xer.Value);
                    await Context.SetKoja("sessionid", res.SessionId);
                }
                await Context.SetKoja("login_response_false", xer.Value);
            }

            return Ok(xer.Beautify());
        }

        [HttpPost]
        public async Task<IActionResult> Logout([FromBody]
            LogOutRequest request)
        {
            var body = Builder
                .UserName()
                .Password()
                .DeviceName()
                .FStream(new { SESSIONID = request.SessionId })
                .Instance;
            var xml = Envelope("AUTH_GetLogOut", body);

            var response = await PostXmlRequest(WebService, xml);
            var source = await response.Content.ReadAsStringAsync();
            var xer = source.XERetrun();

            if (!xer.IsEmpty)
            {
                System.Diagnostics.Debug.WriteLine(xer.Value);
                var res = DeserializeObject<BaseResponse>(xer.Value);
                if (res.Status)
                    await Context.SetKoja("logout_response_true", xer.Value);
                await Context.SetKoja("logout_response_false", xer.Value);
            }

            return Ok(source.XERetrun().Beautify());
        }
    }
}