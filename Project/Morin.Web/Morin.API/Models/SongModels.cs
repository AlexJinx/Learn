using Morin.API.Entity;

namespace Morin.API.Models;

public class CreateSongListInModel
{
    public ulong UserId { get; set; }

    public string SongListName { get; set; }
}

public class GetGetSongListOutModel
{
    public ulong Id { get; set; }

    public ulong UserId { get; set; }

    public string SongListName { get; set; }

    public long AddedAt { get; set; }
}

public class CollectSongInModel
{
    public ulong SongListId { get; set; }

    public PlatformEnum Platform { get; set; }

    public string Song { get; set; }

    public string SongId { get; set; }

    public string Singer { get; set; }

    public uint SingerId { get; set; }

    public string Album { get; set; }

    public uint AlbumId { get; set; }

    public uint Duration { get; set; }

    public int HasMv { get; set; }

    public int HasLoss { get; set; }

    public string ImgUrl { get; set; }

    public string LrcUrl { get; set; }

    public string MP3Url { get; set; }
}

public class GetSongListDetailBySongListIdOutModel
{
    public ulong Id { get; set; }

    public string StrPlatform { get; set; }

    public string Song { get; set; }

    public string SongId { get; set; }

    public string Singer { get; set; }

    public uint SingerId { get; set; }

    public string Album { get; set; }

    public uint AlbumId { get; set; }

    public uint Duration { get; set; }

    public int HasMv { get; set; }

    public int HasLoss { get; set; }

    public string ImgUrl { get; set; }

    public string LrcUrl { get; set; }

    public string MP3Url { get; set; }
}