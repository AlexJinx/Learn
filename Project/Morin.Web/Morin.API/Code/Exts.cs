using System.Text.Encodings.Web;

namespace Morin.API.Code
{
    public static class Exts
    {
        public static int ToInt(this double val)
        {
            return val < int.MinValue ? int.MinValue : val > int.MaxValue ? int.MaxValue : (int)val;
        }

        public static string ToHtmlEncode(this string val, HtmlEncoder encoder)
        {
            return val is { Length: > 0 } ? encoder.Encode(val.Trim()) : string.Empty;
        }

        public static decimal ToDecimal(this double val)
        {
            if (decimal.TryParse(val.ToString(), out decimal result))
            {
                return result;
            }

            return 0;
        }
    }
}
