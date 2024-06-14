using CovidLitSearch.Models;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/journal")]
public class JournalController(IJournalService service) : ControllerBase
{
    /// <summary>
    ///  Get journals by search
    /// </summary>
    /// <param name="search"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet("journals")]
    public async Task<ActionResult<List<Journal>>> GetJournals(
        [FromQuery] string search,
        [FromQuery] int page,
        [FromQuery] int pageSize
    )
    {
        var journals = await service.GetJournals(search, page, pageSize);
        return Ok(journals);
    }
}