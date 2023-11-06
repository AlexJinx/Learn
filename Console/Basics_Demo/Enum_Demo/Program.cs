using System.ComponentModel;

namespace Enum_Demo;

public class Program
{
    static void Main(string[] args)
    {
        List<PhotographyCat> enumList = new()
        {
            PhotographyCat.Issue,
            PhotographyCat.Media
        };

        PhotographyCat enumValue = enumList[0];
        foreach (var item in enumList)
        {
            enumValue |= item;
        }

        var enumRealValue = (byte)enumValue;
        var enumArray = Enum.GetValues(typeof(PhotographyCat)).Cast<PhotographyCat>().Where(x => (enumRealValue & (byte)x) == (byte)x).ToArray();

        Console.WriteLine(enumRealValue);
        Console.WriteLine(string.Join(",", enumArray));
        Console.ReadKey();
    }
}

 
[Flags]
public enum PhotographyCat : byte
{
    /// <summary>
    /// 发布
    /// </summary>
    [Description("发布")]
    Issue = 1,

    /// <summary>
    /// 媒体
    /// </summary>
    [Description("媒体")]
    Media = 2
}