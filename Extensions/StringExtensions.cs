using System.Text.RegularExpressions;

namespace OzonParser.Extensions
{
    public static class StringExtensions
    {
        private const string regexNumberPattern = @"-?\d+(?:\.\d+)?";

        public static int ParseToInt(this string text)
        {
            return int.Parse(Regex.Match(text, regexNumberPattern).Value);
        }

        public static double ParseToDouble(this string text)
        {
            return double.Parse(Regex.Match(text, regexNumberPattern).Value);
        }

        public static ulong ParseToUlong(this string text)
        {
            return ulong.Parse(Regex.Match(text, regexNumberPattern).Value);
        }

        public static decimal ParseToDecimal(this string text)
        {
            return decimal.Parse(Regex.Match(text, regexNumberPattern).Value);
        }
    }
}
