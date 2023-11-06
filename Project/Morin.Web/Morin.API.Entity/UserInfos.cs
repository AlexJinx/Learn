namespace Morin.API.Entity;

public class UserInfo
{
    public ulong Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string Account { get; set; }

    /// <summary>
    /// 登录密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 登录密码加密项
    /// </summary>
    public string PasswordSalt { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public GenderEnum Gender { get; set; }

    public string Birthday { get; set; }

    /// <summary>
    /// 添加时间戳
    /// </summary>
    public long AddedAt { get; set; }

    /// <summary>
    /// 添加设备
    /// </summary>
    public string AddedAgent { get; set; }

    /// <summary>
    /// 添加IP
    /// </summary>
    public string AddedIP { get; set; }
}
