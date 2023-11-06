namespace Morin.API.Entity;

public class SongListInfo
{
    public ulong Id { get; set; }

    /// <summary>
    /// 歌单名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public ulong UserId { get; set; }

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

public class SongListDetailInfo
{
    public ulong Id { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public ulong UserId { get; set; }

    /// <summary>
    /// 歌单Id
    /// </summary>
    public ulong SongListId { get; set; }

    /// <summary>
    /// 歌曲平台
    /// </summary>
    public PlatformEnum Platform { get; set; }

    /// <summary>
    /// 歌曲名
    /// </summary>
    public string Song { get; set; }

    /// <summary>
    /// 歌手Id
    /// </summary>
    public string SongId { get; set; }

    /// <summary>
    /// 歌手
    /// </summary>
    public string Singer { get; set; }

    /// <summary>
    /// 歌手Id
    /// </summary>
    public uint SingerId { get; set; }

    /// <summary>
    /// 专辑
    /// </summary>
    public string Album { get; set; }

    /// <summary>
    /// 专辑Id
    /// </summary>
    public uint AlbumId { get; set; }

    /// <summary>
    /// 歌曲时长
    /// </summary>
    public uint Duration { get; set; }

    /// <summary>
    /// 是否包含Mv
    /// </summary>
    public byte HasMv { get; set; }

    /// <summary>
    /// 是否保护无损音乐
    /// </summary>
    public byte HasLoss { get; set; }

    /// <summary>
    /// 图片路径
    /// </summary>
    public string ImgUrl { get; set; }

    /// <summary>
    /// 歌词路径
    /// </summary>
    public string LrcUrl { get; set; }

    /// <summary>
    /// 歌曲路径
    /// </summary>
    public string MP3Url { get; set; }

    /// <summary>
    /// 添加时间戳
    /// </summary>
    public long AddedAt { get; set; }

    /// <summary>
    /// 添加IP
    /// </summary>
    public string AddedIP { get; set; }

    /// <summary>
    /// 添加设备
    /// </summary>
    public string AddedAgent { get; set; }
}

public class SongLogInfo
{
    public ulong Id { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public ulong UserId { get; set; }

    /// <summary>
    /// 歌曲Id
    /// </summary>
    public ulong SongId { get; set; }

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