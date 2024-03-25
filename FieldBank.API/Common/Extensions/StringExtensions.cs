using System;
using System.Text.RegularExpressions;

namespace Extensions.Web.Core.Extensions
{
    public static class StringExtensions
    {
        private const string SpaceRegex = "\\s";
        private const string Space = " ";

        public static string SuppressSpecialSpaceChar(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return Regex.Replace(str, SpaceRegex, Space);
        }

        public static bool SameAs(this string @string, string stringToCompare)
        {
            return string.Compare(@string, stringToCompare, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static bool? ToNullableBoolean(this string value)
        {
            if (value == null)
            {
                return null;
            }

            if (value.SameAs("true")
                || value.SameAs("yes")
                || value.SameAs("1"))
            {
                return true;
            }

            return false;
        }

        public static bool ToBoolean(this string value)
        {
            return value.ToNullableBoolean() ?? false;
        }

        public static string Truncate(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= length ? value : value.Substring(0, length);
        }

        public static bool HasNoValue(this string value)
        {
            return !HasValue(value);
        }

        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
        
        public static string AddNewLineAfter(this string value)
        {
            return value.Replace(value, value + Environment.NewLine);
        }
        
        public static string AddNewLineBefore(this string value)
        {
            return value.Replace(value, Environment.NewLine + value);
        }
    }
}
