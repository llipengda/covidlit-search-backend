﻿using System.ComponentModel.DataAnnotations;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using CovidLitSearch.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticleController(IArticleService service) : ControllerBase
{
    /// <summary>
    /// Search articles
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="allowNoUrl"></param>
    /// <param name="search"></param>
    /// <param name="searchBy"></param>
    /// <param name="orderBy"></param>
    /// <param name="desc"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="refine"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<ArticleDto>>> GetArticles(
        [FromQuery][Required] int page,
        [FromQuery][Required] int pageSize,
        [FromQuery] bool allowNoUrl = false,
        [FromQuery] string? search = null,
        [FromQuery] ArticleSearchBy? searchBy = null,
        [FromQuery] string? orderBy = null,
        [FromQuery] bool? desc = null,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] string? refine = null
    ) 
    {
        var res = await service.GetArticles(page, pageSize, allowNoUrl, search, searchBy, orderBy, desc, from, to, refine);
        return Ok(res.Unwrap());
    }

    /// <summary>
    ///  Get article by id
    /// </summary>
    /// <param name="id"></param>
    /// <response code="404">Not Found</response>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDto>> GetArticleById([FromRoute] string id)
    {
        var res = await service.GetArticleById(id, User.TryGetId());
        return res.Match<ActionResult<ArticleDto>>(article => Ok(article), _ => NotFound());
    }

    /// <summary>
    /// Get articles by research type, addressed population, challenge, and focus
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="studyType"></param>
    /// <param name="addressedPopulation"></param>
    /// <param name="challenge"></param>
    /// <param name="focus"></param>
    /// <param name="orderBy"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
    [HttpGet("research")]
    public async Task<ActionResult<List<ArticleDto>>> GetArticlesByResearch(
        [FromQuery][Required] int page,
        [FromQuery][Required] int pageSize,
        [FromQuery] string? studyType = null,
        [FromQuery] string? addressedPopulation = null,
        [FromQuery] string? challenge = null,
        [FromQuery] string? focus = null,
        [FromQuery] string? orderBy = null,
        [FromQuery] bool? desc = null
    )
    {
        return (
            await service.GetArticlesByResearch(
                page,
                pageSize,
                studyType,
                addressedPopulation,
                challenge,
                focus,
                orderBy,
                desc
            )
        ).Unwrap();
    }
    
    /// <summary>
    ///  Get articles count
    /// </summary>
    /// <param name="allowNoUrl"></param>
    /// <param name="search"></param>
    /// <param name="searchBy"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="refine"></param>
    /// <returns></returns>
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetArticlesCount(
        [FromQuery] bool allowNoUrl = false,
        [FromQuery] string? search = null,
        [FromQuery] ArticleSearchBy? searchBy = null,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] string? refine = null
    )
    {
        var res = await service.GetArticlesCount(allowNoUrl, search, searchBy, from, to, refine);
        return Ok(res.Unwrap());
    }

    /// <summary>
    ///  Get cites by article id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/cites")]
    public async Task<ActionResult<List<CiteDto>>> GetCites(
        [FromRoute][Required] string id
    )
    {
        return (await service.GetCites(id)).Unwrap();
    }
}
