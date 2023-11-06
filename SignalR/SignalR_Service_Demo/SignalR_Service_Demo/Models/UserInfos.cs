using MessagePack;

namespace SignalR_Service_Demo.Models;

[MessagePackObject(keyAsPropertyName: true)]
public class UserInfo
{
    public ulong Id { get; set; }

    public string Name { get; set; }

    public uint Age { get; set; }

    public IEnumerable<RoleInfo> Roles { get; set; }
}


[MessagePackObject(keyAsPropertyName: true)]
public class RoleInfo
{
    public ulong Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<RoleTinyInfo> RoleTinyInfos { get; set; }
}


[MessagePackObject(keyAsPropertyName: true)]
public class RoleTinyInfo
{
    public ulong Id { get; set; }

    public string Name { get; set; }
}
