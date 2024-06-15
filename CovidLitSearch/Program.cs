using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Profiles;
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

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.SetupSwagger());

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<DbprojectContext>();

builder.Services.AddServices();

var app = builder.Build();

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
