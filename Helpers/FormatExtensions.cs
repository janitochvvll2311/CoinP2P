using System.Text;

namespace CoinP2P.Helpers;

public static class FormatExtensions
{

    public static string GetBase64String(this byte[] bytes)
    {
        var @string = Convert.ToBase64String(bytes);
        return @string;
    }

    public static byte[] GetBase64Array(this string @string)
    {
        var bytes = Convert.FromBase64String(@string);
        return bytes;
    }

    public static string GetString(this byte[] bytes)
    {
        var @string = Encoding.UTF8.GetString(bytes);
        return @string;
    }

    public static byte[] GetBytes(this string @string)
    {
        var bytes = Encoding.UTF8.GetBytes(@string);
        return bytes;
    }

}