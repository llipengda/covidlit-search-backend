using System.ComponentModel.DataAnnotations;
using CovidLitSearch.Models;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorController(IAuthorService service) : ControllerBase
{
    /// <summary>
    /// Get authors by search
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="search"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<Author>>> GetAuthors(
        [Required] int page,
        [Required] int pageSize,
        string? search = null
    )
    {
        return (await service.GetAuthors(search, page, pageSize)).Unwrap();
    }

    /// <summary>
    ///  Get author by id
    /// </summary>
    /// <param name="name"></param>
    /// <response code="404">Not Found</response>
    /// <returns></returns>
    [HttpGet("{name}")]
    public async Task<ActionResult<Author>> GetAuthorById(
        [Required] string name
    )
    {
        var data = await service.GetAuthorById(name);
        return data.Match<ActionResult<Author>>(author => Ok(author), _ => NotFound());
    }
    
    /// <summary>
    /// Get authors count
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetAuthorsCount(
        string? search = null
    )
    {
        return (await service.GetAuthorsCount(search)).Unwrap();
    }
    
}
