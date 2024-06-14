using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/subscribe")]
public class SubscribeController(ISubscribeService service) : ControllerBase
{
    
    /// <summary>
    /// Subscribe to a journal
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="journalName"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Subscribe(int userId, string journalName)
    {
        var result = await service.Subscribe(userId, journalName);
        return result.Match<IActionResult>(
            res => Ok(res),
            error => error.Code switch
            {
                ErrorCode.AlreadySubscribed => Conflict(error),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }
        );
    }

    /// <summary>
    ///  Get all subscribed journals
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetSubscribes(int page, int pageSize, int userId)
    {
        return (await service.GetSubscribes(page, pageSize, userId)).Match<IActionResult>(
            res => Ok(res),
            error => error.Code switch
            {
                ErrorCode.NoData => NoContent(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }
        );
    }
    
    /// <summary>
    ///  Delete a subscribed journal
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="journalName"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> DeleteSubscribe(int userId, string journalName)
    {
        return (await service.DeleteSubscribe(userId, journalName)).Match<IActionResult>(
            _ => NoContent(),
            error => error.Code switch
            {
                ErrorCode.InvalidCredentials => BadRequest(error),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }
        );
    }
}