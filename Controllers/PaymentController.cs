using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static SP2.Helper;

namespace SP2.Controllers
{
  [ApiController]
  [Route("[controller]/[action]")]
  public class PaymentController : ControllerBase
  {
    public PaymentController(IConfiguration _config)
    {
      BillService = _config["BillService"];
    }

    string BillService { get; }

    [HttpGet]
    public async Task<IActionResult> Inquiry([FromQuery] string trxDate,
      [FromQuery] string transDate,
      [FromQuery] string companyCode,
      [FromQuery] string channelID,
      [FromQuery] string billKey,
      [FromQuery] string reference)
    {
      var xml = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:koja=""koja.h2h.billpayment.ws"">
   <soapenv:Header/>
   <soapenv:Body>
      <koja:inquiry>
         <koja:request>
            <koja:language>02</koja:language>
            <koja:trxDateTime>" + trxDate + @"</koja:trxDateTime>
            <koja:transmissionDateTime>" + transDate + @"</koja:transmissionDateTime>
            <koja:companyCode>" + companyCode + @"</koja:companyCode>
            <koja:channelID>" + channelID + @"</koja:channelID>
            <koja:billKey1>" + billKey + @"</koja:billKey1>
            <koja:billKey2></koja:billKey2>
            <koja:billKey3></koja:billKey3>
            <koja:reference1>" + reference + @"</koja:reference1>
            <koja:reference2></koja:reference2>
            <koja:reference3></koja:reference3>
         </koja:request>
      </koja:inquiry>
   </soapenv:Body>
</soapenv:Envelope>";
      
      var message = await PostXmlRequest(BillService, xml);
      var result = await message.Content.ReadAsStringAsync();
      return Ok(result);
    }

    public async Task<IActionResult> Payment([FromQuery] string trxDate,
      [FromQuery] string transDate,
      [FromQuery] string companyCode,
      [FromQuery] string channelID,
      [FromQuery] string billKey,
      [FromQuery] string paidBill,
      [FromQuery] string paymentAmount,
      [FromQuery] string currency,
      [FromQuery] string transactionID,
      [FromQuery] string reference)
    {
      var xml = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:koja=""koja.h2h.billpayment.ws"">
   <soapenv:Header/>
   <soapenv:Body>
      <koja:payment>
         <koja:request> 
            <koja:language>02</koja:language>
            <koja:trxDateTime>" + trxDate + @"</koja:trxDateTime>
            <koja:transmissionDateTime>" + transDate + @"</koja:transmissionDateTime>
            <koja:companyCode>" + companyCode + @"</koja:companyCode>
            <koja:channelID>" + channelID+ @"</koja:channelID>
            <koja:billKey1>" + billKey + @"</koja:billKey1>
            <koja:billKey2></koja:billKey2>
            <koja:billKey3></koja:billKey3>
            <koja:paidBills>
               <koja:string>" + paidBill + @"</koja:string>
            </koja:paidBills>
            <koja:paymentAmount>" + paymentAmount + @"</koja:paymentAmount>
            <koja:currency>" + currency + @"</koja:currency>
            <koja:transactionID>" + transactionID + @"</koja:transactionID>
            <koja:reference1>" + reference + @"</koja:reference1>
            <koja:reference2></koja:reference2>
            <koja:reference3></koja:reference3>
          </koja:request>
      </koja:payment>
   </soapenv:Body>
</soapenv:Envelope>";
      
      var message = await PostXmlRequest(BillService, xml);
      var result = await message.Content.ReadAsStringAsync();
      return Ok();
    }
  }
}