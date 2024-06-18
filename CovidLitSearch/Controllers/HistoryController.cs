using System.ComponentModel.DataAnnotations;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Services.Interface;
using CovidLitSearch.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Authorize]
[Route("api/histories")]
public class HistoryController(IHistoryService service) : ControllerBase
{
    /// <summary>
    /// Get history by user id
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<HistoryDto>>> GetHistory(
        [Required] int page,
        [Required] int pageSize
    )
    {
        return (await service.GetHistory(User.GetId(), page, pageSize)).Unwrap();
    }
}
