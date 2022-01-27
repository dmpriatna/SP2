using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SP2.Data;

namespace SP2.Controllers
{
  [ApiController, Route("[controller]/[action]")]
  public class MasterController : Controller
  {
    public MasterController(IService service)
    {
      Service = service;
    }

    private readonly IService Service;

    [HttpPost]
    public async Task<IActionResult> CancelTransaction([FromBody] CancelTrxRequest request)
    {
      try
      {
        var result = await Service.CancelTransaction(request.Id, request.Reason);
        return Ok(new {
          message = "Transaksi berhasil dibatalkan."
        });
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus([FromBody] SP2StatusRequest request)
    {
      try
      {
        var result = await Service.UpdateStatus(request);
        return Ok(new {
          message = "PositionStatus berhasil diubah.",
          oldValue = result,
          newValue = (int)request.Status
        });
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpPost]
    public async Task<IActionResult> SaveContract([FromBody] ContractDto dto)
    {
      try
      {
        var result = await Service.PutContract(dto);
        return Ok(new {
          ContractId = result
        });
      }
      catch (System.Exception se)
      {
        return new ContentResult
        {
          Content = Newtonsoft.Json.JsonConvert.SerializeObject(new { Message = se.Message }),
          ContentType = "application/json",
          StatusCode = 422
        };
      }
    }

    [HttpPost]
    public async Task<IActionResult> SaveRateContract([FromBody] RateContractDto dto)
    {
      try
      {
        await Service.PutRateContract(dto);
        return Ok();
      }
      catch (System.Exception se)
      {
        return new ContentResult
        {
          Content = Newtonsoft.Json.JsonConvert.SerializeObject(new { Message = se.Message }),
          ContentType = "application/json",
          StatusCode = 422
        };
      }
    }

    [HttpPost]
    public async Task<IActionResult> SaveRatePlatform([FromBody] RatePlatformDto dto)
    {
      try
      {
        await Service.PutRatePlatform(dto);
        return Ok();
      }
      catch (System.Exception se)
      {
        return new ContentResult
        {
          Content = Newtonsoft.Json.JsonConvert.SerializeObject(new { Message = se.Message }),
          ContentType = "application/json",
          StatusCode = 422
        };
      }
    }

    [HttpPost]
    public async Task<IActionResult> SaveSP2(SP2Dto dto)
    {
      try
      {
        var result = await Service.PutSP2(dto);
        return Ok(new {
          JobNumber = result,
          msg = "SP2 berhasil disimpan",
          success = true,
          result = true
        });
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpPost]
    public async Task<IActionResult> SaveTransaction([FromBody] TransactionDto dto)
    {
      try
      {
        await Service.PutTransaction(dto);
        return Ok();
      }
      catch (System.Exception se)
      {
        return new ContentResult
        {
          Content = Newtonsoft.Json.JsonConvert.SerializeObject(new { Message = se.Message }),
          ContentType = "application/json",
          StatusCode = 422
        };
      }
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveTransactionType([FromBody] TransactionTypeDto dto)
    {
      try
      {
        await Service.PutTransactionType(dto);
        return Ok();
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpPost]
    public async Task<IActionResult> SaveInvoice([FromBody] InvoiceDto dto)
    {
      try
      {
        await Service.PutInvoice(dto);
        return Ok();
      }
      catch (System.Exception se)
      {
        return new ContentResult
        {
          Content = Newtonsoft.Json.JsonConvert.SerializeObject(new { Message = se.Message }),
          ContentType = "application/json",
          StatusCode = 422
        };
      }
    }

    [HttpPost]
    public async Task<IActionResult> SaveInvoiceDetail([FromBody] InvoiceDetailDto dto)
    {
      try
      {
        await Service.PutInvoiceDetail(dto);
        return Ok();
      }
      catch (System.Exception se)
      {
        return new ContentResult
        {
          Content = Newtonsoft.Json.JsonConvert.SerializeObject(new { Message = se.Message }),
          ContentType = "application/json",
          StatusCode = 422
        };
      }
    }

    [HttpGet]
    public async Task<IActionResult> ListContract()
    {
      try
      {
        var result = await Service.GetContracts();
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpGet]
    public async Task<IActionResult> ListTransactionType()
    {
      try
      {
        var result = await Service.GetTransactionTypes();
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpGet]
    public async Task<IActionResult> ListSP2(
      [FromQuery] int Start, [FromQuery] int Length,
      [FromQuery] string Search,
      [FromQuery] string PaymentMethod,
      [FromQuery] SP2Status Status,
      [FromQuery] bool? IsJobNumberDesc,
      [FromQuery] bool? IsCreatedDateDesc,
      [FromQuery] bool? IsCompleteDateDesc
    )
    {
      try
      {
        var result = await Service.ListSP2(new ListSP2Request
        {
          Length = Length,
          Start = Start,
          Search = Search,
          PaymentMethod = PaymentMethod,
          Status = ((int)Status),
          Orders = new string[] {
              IsCreatedDateDesc.HasValue ?
                  (IsCreatedDateDesc.Value ? "CreatedDate Desc" : "CreatedDate Asc")
                      : null,
              IsJobNumberDesc.HasValue ?
                  (IsJobNumberDesc.Value ? "JobNumber Desc" : "JobNumber Asc")
                      : null,
              IsCompleteDateDesc.HasValue ?
                  (IsCompleteDateDesc.Value ? "ModifiedDate Desc" : "ModifiedDate Asc")
                      : null
          }
        });
        return Ok(new {
          Data = result.Item1,
          Total = result.Item2
        });
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpGet]
    public async Task<IActionResult> DetailSP2([FromQuery] Guid Id)
    {
      try
      {
        var result = await Service.DetailSP2(Id);
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpGet]
    public async Task<IActionResult> ListTransaction()
    {
      try
      {
        var result = await Service.GetTransactions();
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpGet]
    public async Task<IActionResult> ListRateContract()
    {
      try
      {
        var result = await Service.GetRateContracts();
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpGet]
    public async Task<IActionResult> ListRatePlatform()
    {
      try
      {
        var result = await Service.GetRatePlatforms();
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpGet]
    public async Task<IActionResult> ListInvoice()
    {
      try
      {
        var result = await Service.GetInvoices();
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpGet, Route("{invoiceId:guid}")]
    public async Task<IActionResult> InvoiceDetail([FromRoute] Guid InvoiceId)
    {
      try
      {
        var result = await Service.GetInvoiceDetails(InvoiceId);
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpGet, Produces("application/json"), Route("{npwp}/{blNumber}/{blDate}")]
    public async Task<IActionResult> GetSPPB(
      [FromRoute] string npwp,
      [FromRoute] string blNumber,
      [FromRoute] string blDate
    )
    {
      try
      {
        string result;
        using (var client = new System.Net.Http.HttpClient())
        {
            var message = await client.GetAsync($"https://esbbcext01.beacukai.go.id:9081/NLEMICROAPI-1.0/webresources/ceisa/BC20ByNoBL/{npwp}/{blNumber}/{blDate}");
            result = await message.Content.ReadAsStringAsync();
        }
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpGet]
    public async Task<IActionResult> ListDoSp2(
      [FromQuery] int start,
      [FromQuery] int lenght
    )
    {
      try
      {
        var result = await Service.ListDoSp2(start, lenght);
        return Ok(new
        {
          Data = result.Item1,
          Total = result.Item2
        });
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpPost]
    public async Task<IActionResult> SaveDelegate([FromBody] TrxDelegateDto dto)
    {
      try
      {
        var result = await Service.PutTrxDelegate(dto);
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }
  }
}