using System.ComponentModel.DataAnnotations;
using CovidLitSearch.Models;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/journals")]
public class JournalController(IJournalService service) : ControllerBase
{
    /// <summary>
    ///  Get journals by search
    /// </summary>
    /// <param name="search"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<Journal>>> GetJournals(
        [Required] string search,
        [Required] int page,
        [Required] int pageSize
    )
    {
        return (await service.GetJournals(search, page, pageSize)).Unwrap();
    }
}
