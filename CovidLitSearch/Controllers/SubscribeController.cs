using System.ComponentModel.DataAnnotations;
using CovidLitSearch.Models;
using CovidLitSearch.Services.Interface;
using CovidLitSearch.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Authorize]
[Route("api/subscribes")]
public class SubscribeController(ISubscribeService service) : ControllerBase
{
    /// <summary>
    /// Subscribe to a journal
    /// </summary>
    /// <param name="journalName"></param>
    /// <response code="201">Created</response>
    /// <response code="409">Conflict</response>
    /// <returns></returns>
    [HttpPost("{journalName}")]
    public async Task<ActionResult<Subscribe>> Subscribe(string journalName)
    {
        var result = await service.Subscribe(User.GetId(), journalName);
        return result.Match<ActionResult<Subscribe>>(
            res => Created($"/journal/{journalName}", res),
            err => Conflict(err)
        );
    }

    /// <summary>
    ///  Get all subscribed journals
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<Subscribe>>> GetSubscribes([Required] int page, [Required] int pageSize)
    {
        return (await service.GetSubscribes(page, pageSize, User.GetId())).Unwrap();
    }

    /// <summary>
    ///  Delete a subscribed journal
    /// </summary>
    /// <param name="journalName"></param>
    /// <response code="204">No Content</response>
    /// <response code="400">Bad Request</response>
    /// <returns></returns>
    [HttpDelete("{journalName}")]
    public async Task<ActionResult> DeleteSubscribe(string journalName)
    {
        return (await service.DeleteSubscribe(User.GetId(), journalName)).Match<ActionResult>(
            NoContent,
            BadRequest
        );
    }
}
