using System;
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
        await Service.PutSP2(dto);
        return Ok();
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
  }
}