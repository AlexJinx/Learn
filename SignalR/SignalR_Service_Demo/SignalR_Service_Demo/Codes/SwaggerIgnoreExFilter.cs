using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace SignalR_Service_Demo.Codes;

public class SwaggerIgnoreExFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema?.Properties.Count == 0)
        {
            return;
        }

        var properties = context.Type.GetProperties();

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(Exception))
            {
                schema.Properties.Remove(GetCamelName(property.Name));
            }
        }
    }

    private static string GetCamelName(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return string.Empty;
        }

        return string.Concat(content[0].ToString().ToLower(), content[1..]);
    }
}
