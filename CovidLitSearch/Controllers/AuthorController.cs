using CovidLitSearch.Models;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/author")]
public class AuthorController(IAuthorService service) : ControllerBase
{
    /// <summary>
    /// Get authors by search
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="search"></param>
    /// <returns></returns>
    [HttpGet("authors")]
    public async Task<ActionResult<List<Author>>> GetAuthors(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] string? search = null
    )
    {
        var authors = await service.GetAuthors(search, page, pageSize);
        return Ok(authors);
    }
    
}