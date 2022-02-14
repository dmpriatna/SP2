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
      [FromQuery] string CreatedBy,
      [FromQuery] string FreightForwarderName,
      [FromQuery] SP2Status? Status,
      [FromQuery] bool? IsDelegate,
      [FromQuery] bool? IsJobNumberDesc,
      [FromQuery] bool? IsCreatedDateDesc,
      [FromQuery] bool? IsCompleteDateDesc
    )
    {
      try
      {
        var active = IsDelegate.HasValue && IsDelegate.Value ? new[] { 1, 2, 3, 4 } : new[] { 1, 2, 3 };
        var complete = IsDelegate.HasValue && IsDelegate.Value ? new[] { 5 } : new[] { 4 };
        var stat = Status.HasValue ? (Status == SP2Status.Draft ?
            new int[] { 0 } : (Status == SP2Status.Actived ?
            active : complete)) : new int[]{};
        var result = await Service.ListSP2(new ListSP2Request
        {
          CreatedBy = CreatedBy,
          Length = Length,
          Start = Start,
          Search = Search,
          FreightForwarderName = FreightForwarderName,
          IsDelegate = IsDelegate,
          PaymentMethod = PaymentMethod,
          Status = stat,
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
      [FromQuery] int lenght,
      [FromQuery] string createdBy,
      [FromQuery] string forwarderName,
      [FromQuery] bool? isDelegate
    )
    {
      try
      {
        var result = await Service.ListDoSp2(start, lenght,
          createdBy, forwarderName, isDelegate);
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
    public async Task<IActionResult> SaveDelegate([FromBody] DelegatePayload payload)
    {
      try
      {
        var result = await Service.PutTrxDelegate(payload);
        return Ok(new {
          JobNumber = result
        });
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpGet]
    public async Task<IActionResult> ListDelegate(
      [FromQuery] int start,
      [FromQuery] int length,
      [FromQuery] SP2Status status,
      [FromQuery] string jobNumber,
      [FromQuery] string createdBy,
      [FromQuery] bool? isCreatedDateDesc
      // [FromQuery] bool? isJobNumberDesc,
      // [FromQuery] bool? isCompleteDateDesc
    )
    {
      try
      {
        var request = new TrxDelegateRequest
        {
          CreatedBy = createdBy,
          JobNumber = jobNumber,
          Length = length,
          Start = start,
          Status = status == SP2Status.Draft ?
            new int[] { 0 } : status == SP2Status.Actived ?
            new int[] { 1, 2, 3, 4, 5 } : new int[] { 6 },
          Orders = new string[] {
            isCreatedDateDesc.HasValue ?
              (isCreatedDateDesc.Value ? "CreatedDate Desc" : "CreatedDate Asc")
                : null,
            // isJobNumberDesc.HasValue ?
            //   (isJobNumberDesc.Value ? "JobNumber Desc" : "JobNumber Asc")
            //     : null,
            // isCompleteDateDesc.HasValue ?
            //   (isCompleteDateDesc.Value ? "ModifiedDate Desc" : "ModifiedDate Asc")
            //     : null,
          }
        };
        var result = await Service.GetTrxDelegates(request);
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
    public async Task<IActionResult> DetailDelegate([FromQuery] Guid id)
    {
      try
      {
        var result = await Service.GetTrxDelegate(id);
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatusDelegate(
      [FromQuery] Guid id,
      [FromQuery] int positionStatus,
      [FromQuery] string positionStatusName,
      [FromQuery] string createdBy = ""
    )
    {
      try
      {
        var single = await Service.GetTrxDelegate(id);
        var result = await Service.PutTrxDelegate(new DelegatePayload
        {
          AttorneyLetter = single.AttorneyLetter,
          BLDocument = single.BLDocument,
          ContractNumber = single.ContractNumber,
          CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? single.CreatedBy : createdBy,
          FrieghtForwarderName = single.FrieghtForwarderName,
          Id = single.Id,
          LetterOfIndemnity = single.LetterOfIndemnity,
          NotifyEmails = single.NotifyEmails,
          PositionStatus = positionStatus,
          PositionStatusName = positionStatusName,
          SaveAsDraft = single.SaveAsDraft,
          ServiceName = Enum.Parse<ServiceType>(single.ServiceName),
        });
        return Ok(result);
      }
      catch (System.Exception se)
      {
        throw se;
      }
    }
  }
}