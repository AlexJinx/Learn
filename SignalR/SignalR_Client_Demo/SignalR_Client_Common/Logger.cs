using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR_Client_Common;
public class Logger
{
    private int _status = 0;
    private string _exist = string.Empty;

    private readonly string _path;
    private readonly ConcurrentQueue<KeyValuePair<string, string>> _queue;

    private static readonly Lazy<Logger> lazyInstance = new(() => new Logger());

    public static Logger Instance => lazyInstance.Value;

    private Logger()
    {
        _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        _queue = new();
    }

    public void Write(string topic, string content) => Add(new KeyValuePair<string, string>(topic, content));

    private void Write(KeyValuePair<string, string> log)
    {
        var dateTime = DateTime.Now;
        var path = Path.Combine(_path, dateTime.ToString("yyyyMM"));

        if (!path.Equals(_exist))
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            _exist = path;
        }

        string date = dateTime.Day.ToString().PadLeft(2, '0');
        string file = Path.Combine(path, string.Concat(date, '_', log.Key, ".log"));

        try
        {
            File.AppendAllText(file, string.Concat($"[{DateTime.Now:HH:mm:ss}] ", log.Value, Environment.NewLine), Encoding.UTF8);
        }
        catch { }
    }

    internal static string ToMessage(Exception ex, string title, string content)
    {
        var msg = new StringBuilder(200);
        msg.AppendLine($"[{DateTime.Now:HH:mm:ss}] {title}");

        if (!string.IsNullOrEmpty(content))
        {
            msg.AppendLine("<!-- detail -->");
            msg.AppendLine(content.Trim());
        }

        if (ex != null)
        {
            msg.AppendLine("<!-- exception -->");
            msg.AppendLine(ex.Message);
            msg.AppendLine("<!-- trace -->");
            msg.AppendLine(ex.StackTrace);
            msg.AppendLine("<!-- close -->");
        }

        return msg.ToString();
    }

    private void Add(KeyValuePair<string, string> item)
    {
        _queue.Enqueue(item);

        if (_status != 0)
        {
            return;
        }

        if (Interlocked.CompareExchange(ref _status, 1, 0) != 0)
        {
            return;
        }

        Task.Run(() => Run());
    }

    private void Run()
    {
        while (!_queue.IsEmpty)
        {
            if (!_queue.TryDequeue(out var item))
            {
                continue;
            }

            try
            {
                Write(item);
            }
            catch { }
        }

        _status = 0;
    }
}
