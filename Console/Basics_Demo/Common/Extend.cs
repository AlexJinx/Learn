using System.Text;

namespace Common;

public static class Extend
{
    public static string GetString(this byte[] data)
    {
        if (data == null)
        {
            return string.Empty;
        }
        if (data.Length == 0)
        {
            return string.Empty;
        }
        try
        {
            return Encoding.Default.GetString(data);
        }
        catch
        {
            return string.Empty;
        }
    }

    public static byte[] GetBytes(this string str)
    {
        if (str == null)
        {
            return null;
        }
        if (str.Length == 0)
        {
            return null;
        }
        if (str.Trim().Length == 0)
        {
            return null;
        }
        try
        {
            return Encoding.Default.GetBytes(str);
        }
        catch
        {
            return null;
        }
    }
}