using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/history")]
public class HistoryController(IHistoryService service) : ControllerBase
{
    
    /// <summary>
    /// Get history by user id
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<HistoryDto>>> GetHistory(
        [FromQuery] int userId,
        [FromQuery] int page,
        [FromQuery] int pageSize
    )
    {
        return (await service.GetHistory(userId, page, pageSize)).Match<ActionResult<List<HistoryDto>>>(
            res => Ok(res),
            error => error.Code switch
            {
                ErrorCode.NoData => NoContent(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }
        );
    }
    
}