using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gyldendal.Porter.Common
{
    public static class Extensions
    {
        public static string CheckIfNullThenDefault(this ulong str, string name)
        {
            return str == 0 ? string.Empty : $"{name.SplitWords()}: {str}";
        }

        public static string CheckIfNullThenDefault(this string str, string name)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : $"{name.SplitWords()}: {str}";
        }

        public static string RemoveEmptyLines(this string str)
        {
            return Regex.Replace(str, @"^(\s|\t)+$[\r\n]*", string.Empty, RegexOptions.Multiline);
        }

        public static string SplitWords(this string str)
        {
            var words = Regex.Split(str, "(?<=\\p{Ll})(?=\\p{Lu})|(?<=\\p{L})(?=\\p{Lu}\\p{Ll})");

            var header = string.Join(" ", words);

            return header;
        }

        public static Dictionary<T, string> GetEnumsWithDescription<T>(Type type) where T : Enum
        {
            var dictEnumsWithDescription = new Dictionary<T, string>();
            var enumValues = (T[])Enum.GetValues(type);

            foreach (var enumValue in enumValues)
            {
                var description = GetDescription(enumValue);
                dictEnumsWithDescription.Add(enumValue, description);
            }

            return dictEnumsWithDescription;
        }

        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if (fi is null)
            {
                return string.Empty;
            }

            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}
