using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using MK.Common;

using Morin.API.Business;
using Morin.API.Code;
using Morin.API.Entity;
using Morin.API.Models;

using System.Text.Encodings.Web;

namespace Morin.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SongController : APIBase
{
    private readonly MorinConfigModel _mrCfg;

    private readonly UserBusiness _userBiz;
    private readonly SongBusiness _songBiz;
    private readonly SongBusiness _songBizW;

    private readonly HtmlEncoder _htmlEncoder;

    public SongController(HtmlEncoder htmlEncoder, IOptions<MorinConfigModel> mrCfg)
    {
        _mrCfg = mrCfg.Value;
        _htmlEncoder = htmlEncoder;
        _userBiz = new UserBusiness(mrCfg.Value.MySQLConn);
        _songBizW = new SongBusiness(mrCfg.Value.MySQLConnWrite);
    }


    [HttpPost("songlist")]
    public async Task<MKResult> CreateSongListAsync(CreateSongListInModel model)
    {
        if (model is null)
        {
            return new MKResult(400, "非法参数");
        }

        if (string.IsNullOrWhiteSpace(model.SongListName))
        {
            return new MKResult(400, "请填写歌单名称");
        }

        var userInfo = await _userBiz.GetUserInfoAsync(model.UserId);
        if (userInfo.TotalCount is 0)
        {
            return new MKResult(400, "未查询到用户信息");
        }

        InitAPI();
        var info = new SongListInfo
        {
            Id = MKHelper.GetULongID(),
            Name = _htmlEncoder.Encode(model.SongListName),
            UserId = userInfo.Body.Id,
            AddedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            AddedIP = CurrentUser.RemoteIP,
            AddedAgent = CurrentUser.UserAgent
        };
        return await _songBizW.InsertSongListAsync(info);
    }

    [HttpPatch("songlist/name")]
    public async Task<MKResult> UpdateSongListNameAsync(CreateSongListInModel model)
    {
        if (model is null)
        {
            return new MKResult(400, "非法参数");
        }

        if (string.IsNullOrWhiteSpace(model.SongListName))
        {
            return new MKResult(400, "请填写歌单名称");
        }

        var userInfo = await _userBiz.GetUserInfoAsync(model.UserId);
        if (userInfo.TotalCount is 0)
        {
            return new MKResult(400, "未查询到用户信息");
        }

        InitAPI();
        var info = new SongListInfo
        {
            Id = MKHelper.GetULongID(),
            Name = _htmlEncoder.Encode(model.SongListName),
            UserId = userInfo.Body.Id,
            AddedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            AddedIP = CurrentUser.RemoteIP,
            AddedAgent = CurrentUser.UserAgent
        };
        return await _songBizW.InsertSongListAsync(info);
    }

    [HttpDelete("songlist")]
    public async Task<MKResult> DeleteSongListAsync(ulong[] songListIds)
    {
        return await _songBizW.DeleteSongListAsync(songListIds);
    }

    [HttpGet("songlist/user/{userId}")]
    public async Task<MKResult<IEnumerable<GetGetSongListOutModel>>> GetGetSongListAsync(ulong userId)
    {
        if (userId is 0)
        {
            return new MKResult<IEnumerable<GetGetSongListOutModel>>(code: 400, msg: "非法参数");
        }

        var userInfo = await _userBiz.GetUserInfoAsync(userId);
        if (userInfo.TotalCount is 0)
        {
            return new MKResult<IEnumerable<GetGetSongListOutModel>>(code: 400, msg: "未查询到用户信息");
        }

        var songList = await _songBiz.GetSongListByUserIdAsync(userId);
        var result = songList.Body.Select(c => new GetGetSongListOutModel
        {
            Id = c.Id,
            UserId = c.UserId,
            SongListName = c.Name,
            AddedAt = c.AddedAt
        });

        return new MKResult<IEnumerable<GetGetSongListOutModel>>(result, songList.TotalCount);
    }

    [HttpPost]
    public async Task<MKResult> CollectSongAsync(CollectSongInModel model)
    {
        if (model is null)
        {
            return new MKResult(400, "非法参数");
        }

        if (model.SongListId is 0 ||
            model.Duration is 0 ||
           string.IsNullOrWhiteSpace(model.Song) ||
           string.IsNullOrWhiteSpace(model.SongId) ||
           string.IsNullOrWhiteSpace(model.Singer) ||
           string.IsNullOrWhiteSpace(model.Album))
        {
            return new MKResult(400, "非法参数");
        }

        InitAPI();
        var info = new SongListDetailInfo
        {
            Id = MKHelper.GetULongID(),
            UserId = CurrentUser.Id,
            SongListId = model.SongListId,
            Platform = model.Platform,
            Song = model.Song,
            SongId = model.SongId,
            Singer = model.Singer,
            SingerId = model.SingerId,
            Album = model.Album,
            AlbumId = model.AlbumId,
            Duration = model.Duration,
            HasMv = (byte)model.HasMv,
            HasLoss = (byte)model.HasLoss,
            ImgUrl = model.ImgUrl,
            LrcUrl = model.LrcUrl,
            MP3Url = model.MP3Url,
            AddedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            AddedIP = CurrentUser.RemoteIP,
            AddedAgent = CurrentUser.UserAgent
        };

        return await _songBizW.CollectSongAsync(info);
    }

    [HttpDelete("{detailId}")]
    public async Task<MKResult> UnCollectSongAsync(ulong detailId)
    {
        if (detailId is 0)
        {
            return new MKResult(400, "非法参数");
        }

        return await _songBizW.UnCollectSongAsync(detailId);
    }

    [HttpGet("{songListId}")]
    public async Task<MKResult<IEnumerable<GetSongListDetailBySongListIdOutModel>>> GetSongListDetailBySongListIdAsync(ulong songListId)
    {
        if (songListId is 0)
        {
            return new MKResult<IEnumerable<GetSongListDetailBySongListIdOutModel>>(code: 400, msg: "非法参数");
        }

        var songListInfo = await _songBiz.GetSongListBySongListIdAsync(songListId);

        if (songListInfo.TotalCount == 0)
        {
            return new MKResult<IEnumerable<GetSongListDetailBySongListIdOutModel>>(code: 400, msg: "未查询到歌单信息");
        }

        var detail = await _songBiz.GetSongListDetailBySongListIdAsync(songListId);
        var result = detail.Body.Select(c => new GetSongListDetailBySongListIdOutModel
        {
            Id = c.Id,
            StrPlatform = c.Platform.GetDesc(),
            Song = c.Song,
            SongId = c.SongId,
            Singer = c.Singer,
            SingerId = c.SingerId,
            Album = c.Album,
            AlbumId = c.AlbumId,
            Duration = c.Duration,
            HasMv = c.HasMv,
            HasLoss = c.HasLoss,
            ImgUrl = c.ImgUrl,
            LrcUrl = c.LrcUrl,
            MP3Url = c.MP3Url
        });

        return new MKResult<IEnumerable<GetSongListDetailBySongListIdOutModel>>(result, detail.TotalCount);
    }

    [HttpPost("log")]
    public async Task<MKResult> CreateSongLogAsync(ulong songid)
    {
        InitAPI();
        var info = new SongLogInfo
        {
            Id = MKHelper.GetULongID(),
            UserId = CurrentUser.Id,
            SongId = songid,
            AddedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            AddedIP = CurrentUser.RemoteIP,
            AddedAgent = CurrentUser.UserAgent
        };

        return await _songBizW.InsertSongLogAsync(info);
    }


}