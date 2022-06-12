using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gyldendal.Porter.Application.Contracts;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Gyldendal.Porter.Api.Filters
{
    public class SwaggerNullablePayloadFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context?.Type == null)
                return;
            var nullableGenericProperties = context.Type.GetProperties()
                .Where(t =>
                    t.GetCustomAttribute<AllowNullAttribute>()
                    != null);
            foreach (var excludedProperty in nullableGenericProperties)
            {
                if (schema.Properties.ContainsKey(excludedProperty.Name.ToLowerInvariant()))
                {
                    var prop = schema.Properties[excludedProperty.Name.ToLowerInvariant()];
                    prop.Nullable = true;
                    prop.Required = new HashSet<string> { "false" };
                }
            }
        }
    }
}
