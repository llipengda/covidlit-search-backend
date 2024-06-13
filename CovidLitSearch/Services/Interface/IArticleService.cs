﻿using CovidLitSearch.Models;

namespace CovidLitSearch.Services.Interface;

public interface IArticleService
{
    Task<List<Article>> GetArticles();
}