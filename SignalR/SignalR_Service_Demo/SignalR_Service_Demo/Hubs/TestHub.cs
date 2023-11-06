using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using MK.Common;

using SignalR_Service_Demo.Models;

namespace SignalR_Service_Demo.Hubs;

// 必须放在类上面，不可放在方法上
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TestHub : Hub
{
    public async Task TestModelAsync()
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

        await Clients.Caller.SendAsync("SendModelMessage", userInfo);
    }

    public async Task SendMessageToAllClientAsync()
    {
        await Clients.All.SendAsync("SendStrMessage", "Msg");
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task ReceiveMsgFromClientAsync(UserInfo model)
    {
        var inModel = model;
        Console.WriteLine(inModel.ToJson());
        await Task.CompletedTask;
    }

    public override Task OnConnectedAsync()
    {
        var connId = Context.ConnectionId;
        return base.OnConnectedAsync();
    }
}