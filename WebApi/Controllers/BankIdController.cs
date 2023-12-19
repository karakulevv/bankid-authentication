using Application.Exceptions;
using Application.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace WebApi.Controllers;

[Route("/apiauth/bankid")]
public class BankIdController : Controller
{
    private readonly IMediator _mediator;

    public BankIdController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("initiate")]
    [Produces("application/json")]
    //[ProducesResponseType(typeof(SimplifiedBankIdStartResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Initiate(StartRequest startRequest)
    {
        if (!ModelState.IsValid)
            return HandleModelStateError(ModelState);

        var response = await _mediator.Send(startRequest);

        return Ok(response);
    }

    [HttpPost("initiate")]
    [Produces("application/json")]
    //[ProducesResponseType(typeof(SimplifiedBankIdStartResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Collect(CollectRequest collectRequest)
    {
        if (!ModelState.IsValid)
            return HandleModelStateError(ModelState);

        var response = await _mediator.Send(collectRequest);

        return Ok(response);
    }

    [HttpPost("cancel")]
    [Produces("application/json")]
    //[ProducesResponseType(typeof(SimplifiedBankIdStartResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Cancel(CancelRequest cancelRequest)
    {
        if (!ModelState.IsValid)
            return HandleModelStateError(ModelState);

        await _mediator.Send(cancelRequest);

        return Ok();
    }

    [HttpPost("qrcode/generate")]
    [Produces("application/json")]
    //[ProducesResponseType(typeof(SimplifiedBankIdStartResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> QRGenerate(QrCodeRequest qrCodeRequest)
    {
        if (!ModelState.IsValid)
            return HandleModelStateError(ModelState);

        var response = await _mediator.Send(qrCodeRequest);

        return Ok(response);
    }

    private IActionResult HandleModelStateError(ModelStateDictionary modelState)
    {
        string errors = string.Join("; ", modelState.Values
                                                  .SelectMany(x => x.Errors)
                                                  .Select(x => x.ErrorMessage));
        return BadRequest(new HttpResponseException("invalid-request-data", 400, errors));
    }
}