using System.ComponentModel.DataAnnotations;
using CovidLitSearch.Models;
using CovidLitSearch.Models.DTO;
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
    /// <param name="refine"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<Journal>>> GetJournals(
        [Required] string search,
        [Required] int page,
        [Required] int pageSize,
        string? refine = null
    )
    {
        return (await service.GetJournals(search, page, pageSize, refine)).Unwrap();
    }
    
    /// <summary>
    ///  Get journal by id
    /// </summary>
    /// <param name="name"></param>
    /// <response code="404">Not Found</response>
    /// <returns></returns>
    [HttpGet("{name}")]
    public async Task<ActionResult<Journal>> GetJournalById(
        [Required] string name
    )
    {
        var data = await service.GetJournalById(name);
        return data.Match<ActionResult<Journal>>(journal => Ok(journal), _ => NotFound());
    }
    
    /// <summary>
    /// Get journals count
    /// </summary>
    /// <param name="search"></param>
    /// <param name="refine"></param>
    /// <returns></returns>
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetJournalsCount(
        string? search = null,
        string? refine = null
    )
    {
        return (await service.GetJournalsCount(search, refine)).Unwrap();
    }
    
    /// <summary>
    ///  Get articles by journal
    /// </summary>
    /// <param name="name"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <response code="404">Not Found</response>
    /// <returns></returns>
    [HttpGet("{name}/articles")]
    public async Task<ActionResult<List<ArticleDto>>> GetArticlesByJournal(
        [Required] string name,
        [Required] int page,
        [Required] int pageSize
    )
    {
        var data = await service.GetArticlesByJournal(name, page, pageSize);
        return data.Match<ActionResult<List<ArticleDto>>>(articles => Ok(articles), _ => NotFound());
    }
    
}
