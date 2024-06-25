using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CovidLitSearch.Utilities.Filters;

public class EnumDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var dict = GetAllEnum();

        foreach (var (typeName, property) in swaggerDoc.Components.Schemas)
        {
            if (property.Enum is null || property.Enum.Count <= 0)
            {
                continue;
            }

            var itemType = dict[typeName];
            List<OpenApiInteger> list = [];
            list.AddRange(property.Enum.Cast<OpenApiInteger>());
            property.Description += DescribeEnum(itemType, list);
        }
    }

    private static Dictionary<string, Type> GetAllEnum()
    {
        var ass = Assembly.Load("CovidLitSearch");
        var types = ass.GetTypes();
        Dictionary<string, Type> dict = [];

        foreach (var item in types)
        {
            if (item.IsEnum)
            {
                dict.Add(item.Name, item);
            }
        }

        return dict;
    }

    private static string DescribeEnum(Type type, List<OpenApiInteger> enums)
    {
        var enumDescriptions = new List<string>();
        foreach (var item in enums)
        {
            var value = Enum.Parse(type, item.Value.ToString());
            enumDescriptions.Add($"{Enum.GetName(type, value)} = {item.Value}");
        }

        return $"<br/>{Environment.NewLine}{string.Join("<br/>" + Environment.NewLine, enumDescriptions)}";
    }
}