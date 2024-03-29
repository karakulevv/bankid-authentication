﻿using Application.Exceptions;
using Application.Models.Requests;
using Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace WebApi.Controllers;

[Route("/apiauth/bankid")]
[ApiController]
public class BankIdController : ControllerBase
{
    private readonly IMediator _mediator;

    public BankIdController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("initiate")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(StartResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Initiate([FromBody] StartRequest startRequest)
    {
        if (!ModelState.IsValid)
            return HandleModelStateError(ModelState);

        var response = await _mediator.Send(startRequest);

        return Ok(response);
    }

    [HttpPost("collect")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CollectResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Collect([FromBody] CollectRequest collectRequest)
    {
        if (!ModelState.IsValid)
            return HandleModelStateError(ModelState);

        var response = await _mediator.Send(collectRequest);

        return Ok(response);
    }

    [HttpPost("cancel")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Cancel([FromBody] CancelRequest cancelRequest)
    {
        if (!ModelState.IsValid)
            return HandleModelStateError(ModelState);

        await _mediator.Send(cancelRequest);

        return Ok();
    }

    [HttpPost("qrcode/generate")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(QrCodeResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> QRGenerate([FromBody] QrCodeRequest qrCodeRequest)
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