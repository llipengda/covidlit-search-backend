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
}
