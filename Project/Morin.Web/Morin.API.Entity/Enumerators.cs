using System.ComponentModel;

namespace Morin.API.Entity;

/// <summary>
/// 平台类别
/// </summary>
[Flags]
public enum PlatformEnum : byte
{
    /// <summary>
    /// 扣扣音乐
    /// </summary>
    [Description("扣扣音乐")]
    CCMusic = 0b0000,

    /// <summary>
    /// 往易芸音乐
    /// </summary>
    [Description("往易芸音乐")]
    WYYMusic = 0b0001,

    /// <summary>
    /// 酷硪音乐
    /// </summary>
    [Description("酷硪音乐")]
    CWMusic = 0b0010,

    /// <summary>
    /// 酷苟音乐
    /// </summary>
    [Description("酷苟音乐")]
    CGMusic = 0b0100,
}

/// <summary>
/// 性别
/// </summary>
public enum GenderEnum : byte
{
    /// <summary>
    /// 女
    /// </summary>
    [Description("女")]
    FeMale = 0,

    /// <summary>
    /// 男
    /// </summary>
    [Description("男")]
    Male = 1
}
