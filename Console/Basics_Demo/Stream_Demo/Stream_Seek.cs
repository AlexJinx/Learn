using System.Text;

using Common;

namespace Stream_Demo;
public class Stream_Seek
{
    public static void Master()
    {
        byte[] read;
        var str = "123456789";
        var data = Encoding.Default.GetBytes(str);

        MemoryStream ms = new(data);

        //ms.Position = 4;
        Console.WriteLine($" Index Of Befour Read：{ms.Position} ");
        read = new byte[3];
        ms.Seek(2, SeekOrigin.Begin);
        ms.Read(read, 0, 3);
        Console.WriteLine($" Seek {read.Length} Length And SeekOrigin.Begin：{read.GetString()} ");
        Console.WriteLine($" Index Of After Read： {ms.Position}{Environment.NewLine} ");
        //从流的第一个开始读，跳过前两个读取3个字节

        //ms.Position = 0;
        Console.WriteLine($" Index Of Befour Read：{ms.Position} ");
        read = new byte[3];
        ms.Seek(0, SeekOrigin.Current);
        ms.Read(read, 0, 3);
        Console.WriteLine($" Seek {read.Length} Length And SeekOrigin.Current：{read.GetString()} ");
        Console.WriteLine($" Index Of After Read： {ms.Position}{Environment.NewLine} ");
        //从流的当前位置开始读，不跳过，读取3个字节

        //ms.Position = 0;
        Console.WriteLine($" Index Of Befour Read： {ms.Position}  ");
        read = new byte[3];
        ms.Seek(-5, SeekOrigin.End);
        ms.Read(read, 0, 3);
        Console.WriteLine($" Seek {read.Length} Length And SeekOrigin.End：{read.GetString()} ");
        Console.WriteLine($" Index Of After Read： {ms.Position}{Environment.NewLine} ");
        //从流的末尾倒着读取指定个数的字节，然后从这一段的最开头读取3个字节

        Console.ReadKey();
    }
}