using MessagePack;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

using MK.Common;

using SignalR_Client_Common;

using SignalR_Client_Demo.Models;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SignalR_Client_Demo;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    public string _token;

    private readonly HttpClient _httpCli;

    private HubConnection _hub;
    public MainWindow(IHttpClientFactory httpClientFactory)
    {
        _httpCli = httpClientFactory.CreateClient();

        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await InitHubAsync();
    }

    public async Task InitHubAsync()
    {
        if (_hub != null)
        {
            await _hub.DisposeAsync();
        }

        await VerifyToken();

        _hub = new HubConnectionBuilder()
        .WithUrl($"{ConfigurationManager.AppSettings["SeverUrl"]}/testHub", opts =>
        {
            opts.AccessTokenProvider = async () => await GetTokenAsync();
            opts.Headers["Authorization"] = _token; // Token默认放在URL中的,可以加上这句放在Header中,服务端也要改成从Header中获取Token（好像不行...）,不需要加 Bearer 前缀
        })
        .AddMessagePackProtocol(options =>
        {
            options.SerializerOptions = MessagePackSerializerOptions
                        .Standard
                        .WithResolver(MessagePack.Resolvers.StandardResolver.Instance)
                        .WithSecurity(MessagePackSecurity.UntrustedData);
        })
        .WithAutomaticReconnect(new RetryPolicy())
        .Build();
        _hub.Closed += HubClosed;
        _hub.Reconnected += HubReconnected;
        _hub.Reconnecting += HubReconnecting;

        await _hub.StartAsync();
        await InitListen();
    }

    private async Task InitListen()
    {
        _hub.On<UserInfo>("SendModelMessage", data =>
        {
            var cc = data;
        });

        _hub.On<UserInfo>("SendStrMessage", data =>
        {
            var cc = data;
        });

        await Task.CompletedTask;
    }

    private async Task HubClosed(Exception arg)
    {
        Logger.Instance.Write("连接服务器异常", $"已关闭连接:{DateTime.Now}");
        await _hub.StartAsync();
    }

    private async Task HubReconnected(string arg)
    {
        Logger.Instance.Write("连接服务器异常", $"已成功重新连接:{DateTime.Now}");
        //await InitListen();
        await Task.CompletedTask;
    }

    private async Task HubReconnecting(Exception arg)
    {
        Logger.Instance.Write("连接服务器异常", $"已与服务器断开连接,正在尝试重连:{DateTime.Now}");
        await Task.CompletedTask;
    }

    private async Task<string> GetTokenAsync()
    {
        var url = $"{ConfigurationManager.AppSettings["SeverUrl"]}/api/test/token";
        var token = await _httpCli.GetStringAsync(url);
        _token = token;
        return token;
    }

    private async Task VerifyToken()
    {
        _httpCli.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {_token}");
        var url = $"{ConfigurationManager.AppSettings["SeverUrl"]}/api/test/token/verify";
        var res = await _httpCli.GetAsync(url);
        if (res.StatusCode == HttpStatusCode.Unauthorized)
        {
            _ = await GetTokenAsync();
        }
    }

    public async Task SendMsgToServiceAsync()
    {
        try
        {
            var userInfo = new UserInfo
            {
                Id = MKHelper.GetULongID(),
                Name = "Service",
                Age = 25,
                Roles = new[]
                {
                new RoleInfo
                {
                    Id  = MKHelper.GetULongID(),
                    Name ="One",
                    RoleTinyInfos = new[]
                        {
                            new RoleTinyInfo
                            {
                                Id = MKHelper.GetULongID(),
                                Name= "Tiny"
                            },
                            new RoleTinyInfo
                            {
                                Id = MKHelper.GetULongID(),
                                Name= "Tiny"
                            }
                        }
                    },
                    new RoleInfo
                    {
                        Id  = MKHelper.GetULongID(),
                        Name ="Two",
                           RoleTinyInfos = new[]
                         {
                             new RoleTinyInfo
                             {
                                 Id = MKHelper.GetULongID(),
                                  Name= "Tiny"
                             },
                              new RoleTinyInfo
                             {
                                 Id = MKHelper.GetULongID(),
                                  Name= "Tiny"
                             }
                         }
                    }
                }
            };

            await _hub.InvokeAsync("ReceiveMsgFromClientAsync", userInfo);
        }
        catch (Exception ex)
        {
            Logger.Instance.Write("Receive异常", $"未成功发送数据到服务器：{ex.Message}");
        }
    }

    private async void btn_One_Click(object sender, RoutedEventArgs e)
    {
        await SendMsgToServiceAsync();
    }
}
