using CovidLitSearch.Models;
using CovidLitSearch.Models.Enums;
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
        return (await service.GetAuthors(search, page, pageSize)).Match<ActionResult<List<Author>>>(
            res => Ok(res),
            error => error.Code switch
            {
                ErrorCode.NoData => NoContent(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }
        );
    }
    
}