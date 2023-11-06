using Microsoft.AspNetCore.SignalR.Client;

using SignalR_Client_Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR_Client_Demo;
public class RetryPolicy : IRetryPolicy
{
    /// <summary>
    /// 重连规则：重连次数<50：间隔3s;重试次数<250:间隔30s;重试次数>250:间隔1m
    /// </summary>
    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        var count = retryContext.PreviousRetryCount;
        if (count < 10)//重试次数 < 10,间隔1s
        {
            Logger.Instance.Write("连接服务器异常", $"正在重新连接,重连时间{DateTime.Now}");
            return new TimeSpan(0, 0, 3);
        }
        else if (count < 50)//重试次数 < 50:间隔30s
        {
            Logger.Instance.Write("连接服务器异常", $"正在重新连接,重连时间{DateTime.Now}");
            return new TimeSpan(0, 0, 30);
        }
        else //重试次数 > 50:间隔1m
        {
            Logger.Instance.Write("连接服务器异常", $"正在重新连接,重连时间{DateTime.Now}");
            return new TimeSpan(0, 1, 0);
        }
    }
}