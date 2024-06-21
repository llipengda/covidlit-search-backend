using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Profiles;
using CovidLitSearch.Utilities;
using CovidLitSearch.Utilities.Filters;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

AppSettings.Init(builder.Configuration);

builder
    .Services.AddControllers(options => options.Filters.Add<LoggingFilter>())
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
            .Json
            .ReferenceLoopHandling
            .Ignore
    );

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.SetupSwagger());

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddCors(options => options.AddDefaultPolicy(
    policy =>
    {
        if (builder.Configuration["Cors:Origins"] is { } origins && !string.IsNullOrEmpty(origins))
        {
            policy.WithOrigins(origins.Split(",")).AllowAnyMethod().AllowAnyHeader();
        }
        else
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    }));

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<DbprojectContext>();

builder.Services.AddServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


app.UseAuthorization();

app.MapControllers();

app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));

app.UseCors();

app.Run();