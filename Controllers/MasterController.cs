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

    [HttpGet]
    public async Task<IActionResult> ListTransaction()
    {
      try
      {
        var result = await Service.Transactions();
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
        var result = await Service.Invoices();
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpGet]
    public async Task<IActionResult> InvoiceDetail(Guid InvoiceId)
    {
      try
      {
        var result = await Service.Invoices();
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }
  }
}