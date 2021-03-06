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
                if (IsActive.Status)
                    return Ok(IsActive);
                await InLogout(new LogOutRequest { SessionId = SessionId });
            }

            var Encrypt = EncryptMD5(request.Password);
            var body = Builder
                .UserName(request.Username)
                .Password(request.Password)
                .FStream(new { PASSWORD = Encrypt, USERNAME = request.PhoneNumber })
                .DeviceName()
                .Instance;
            var xml = Envelope("AUTH_GetLogin", body);

            var response = await PostXmlRequest(WebService, xml);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);
            var source = await response.Content.ReadAsStringAsync();
            var xer = source.XERetrun();

            if (!xer.IsEmpty)
            {
                var res = DeserializeObject<LoginResponse>(xer.Value);
                if (res.Status)
                {
                    await Context.SetKoja("auth_getlogin_true", xer.Value);
                    await Context.SetKoja("sessionid", res.SessionId);
                }
                else
                    await Context.SetKoja("auth_getlogin_false", xer.Value);
                return Ok(res);
            }

            return Ok(new LoginResponse {
                Message = "AUTH_GetLogin : response does not have block call return" });
        }

        private async Task<System.Xml.Linq.XElement> InLogout(LogOutRequest request)
        {
            var body = Builder
                .UserName()
                .Password()
                .FStream(new { SESSIONID = request.SessionId })
                .DeviceName()
                .Instance;
            var xml = Envelope("AUTH_GetLogOut", body);

            var response = await PostXmlRequest(WebService, xml);
            var source = await response.Content.ReadAsStringAsync();
            var xer = source.XERetrun();

            if (!xer.IsEmpty)
            {
                var res = DeserializeObject<BaseResponse>(xer.Value);
                if (res.Status)
                    await Context.SetKoja("auth_getlogout_true", xer.Value);
                else
                    await Context.SetKoja("auth_getlogout_false", xer.Value);
            }

            return xer;
        }

        [HttpPost]
        public async Task<IActionResult> Logout([FromBody]
            LogOutRequest request)
        {
            var xer = await InLogout(request);
            return Ok(xer.Beautify());
        }
    }
}