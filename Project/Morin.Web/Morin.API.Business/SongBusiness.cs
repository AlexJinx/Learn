using MK.Common;

using Morin.API.Data;
using Morin.API.Entity;

namespace Morin.API.Business;

public class SongBusiness
{
    private readonly SongData _songData;

    public SongBusiness(string connStr)
    {
        _songData = new SongData(connStr);
    }


    #region SongList

    public async Task<MKResult> InsertSongListAsync(SongListInfo info)
    {
        try
        {
            var result = await _songData.InsertSongListAsync(info);
            if (result > 0)
            {
                return new MKResult(200, "新增歌单成功");
            }
            return new MKResult(200, "新增歌单失败");
        }
        catch (Exception e)
        {
            return new MKResult(500, e.Message, e);
        }
    }

    public async Task<MKResult> UpdateSongListNameAsync(ulong songListId, string name)
    {
        try
        {
            var result = await _songData.UpdateSongListNameAsync(songListId, name);
            if (result > 0)
            {
                return new MKResult(200, "修改歌单名称成功");
            }
            return new MKResult(200, "修改歌单名称失败");
        }
        catch (Exception e)
        {
            return new MKResult(500, e.Message, e);
        }
    }

    public async Task<MKResult> DeleteSongListAsync(ulong[] songListIds)
    {
        try
        {
            var result = await _songData.DeleteSongListAsync(songListIds);
            if (result)
            {
                return new MKResult(200, "删除歌单名称成功");
            }
            return new MKResult(200, "删除歌单名称失败");
        }
        catch (Exception e)
        {
            return new MKResult(500, e.Message, e);
        }
    }

    public async Task<MKResult<IEnumerable<SongListInfo>>> GetSongListBySongListIdAsync(ulong songListId)
    {
        try
        {
            var result = await _songData.GetSongListBySongListIdAsync(songListId);
            return new MKResult<IEnumerable<SongListInfo>>(result, result.Count());
        }
        catch (Exception e)
        {
            return new MKResult<IEnumerable<SongListInfo>>(code: 500, msg: e.Message, ex: e);
        }
    }

    public async Task<MKResult<IEnumerable<SongListInfo>>> GetSongListByUserIdAsync(ulong userId)
    {
        try
        {
            var result = await _songData.GetSongListByUserIdAsync(userId);
            return new MKResult<IEnumerable<SongListInfo>>(result, result.Count());
        }
        catch (Exception e)
        {
            return new MKResult<IEnumerable<SongListInfo>>(code: 500, msg: e.Message, ex: e);
        }
    }



    #endregion

    #region SongListDetail

    public async Task<MKResult> CollectSongAsync(SongListDetailInfo info)
    {
        try
        {
            var result = await _songData.CollectSongAsync(info);
            if (result > 0)
            {
                return new MKResult(200, "收藏成功");
            }
            return new MKResult(200, "收藏失败");
        }
        catch (Exception e)
        {
            return new MKResult(500, e.Message, e);
        }
    }

    public async Task<MKResult> UnCollectSongAsync(ulong detailId)
    {
        try
        {
            var result = await _songData.UnCollectSongAsync(detailId);
            if (result > 0)
            {
                return new MKResult(200, "取消收藏成功");
            }
            return new MKResult(200, "取消收藏失败");
        }
        catch (Exception e)
        {
            return new MKResult(500, e.Message, e);
        }
    }

    public async Task<MKResult<IEnumerable<SongListDetailInfo>>> GetSongListDetailBySongListIdAsync(ulong songListId)
    {
        try
        {
            var result = await _songData.GetSongListDetailBySongListIdAsync(songListId);
            return new MKResult<IEnumerable<SongListDetailInfo>>(result, result.Count());
        }
        catch (Exception e)
        {
            return new MKResult<IEnumerable<SongListDetailInfo>>(code: 500, msg: e.Message, ex: e);
        }
    }

    #endregion

    #region SongLog

    public async Task<MKResult> InsertSongLogAsync(SongLogInfo info)
    {
        try
        {
            var result = await _songData.InsertSongLogAsync(info);
            if (result > 0)
            {
                return new MKResult(200, "执行成功");
            }
            return new MKResult(200, "执行失败");
        }
        catch (Exception e)
        {
            return new MKResult(500, e.Message, e);
        }
    }

    #endregion
}
