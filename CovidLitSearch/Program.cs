using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Services;
using CovidLitSearch.Services.Interface;
using CovidLitSearch.Utilities;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

AppSettings.Init(builder.Configuration);

builder
    .Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
            .Json
            .ReferenceLoopHandling
            .Ignore
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.SetupSwagger());

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<DbprojectContext>();

builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));

app.Run();
