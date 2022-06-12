using System;
using System.Linq;
using System.Reflection;
using MongoDB.Driver;

namespace Gyldendal.Porter.Infrastructure.Repository.HelperExtensions
{
    public static class RepositoryHelper
    {
        public static FindOptions<T> GetProjectionFilter<T>(this string properties)
        {
            var propertiesToInclude = properties?.Split(separator: new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());

            var options = new FindOptions<T>();

            if (propertiesToInclude is null)
            {
                return options;
            }

            var fields = "";

            foreach (var property in propertiesToInclude)
            {
                var value = GetPropertyValue<T>(property);
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = property;
                }

                fields += $"'{value}': 1,";
            }

            options.Projection = $"{{{fields}}}";

            return options;
        }

        private static string GetPropertyValue<T>(string propertyName)
        {
            return typeof(T)
                .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                ?.Name;
        }
    }
}
