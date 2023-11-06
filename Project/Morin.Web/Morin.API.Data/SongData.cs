using Dapper;

using Morin.API.Entity;

using MySql.Data.MySqlClient;

using System.Data;

namespace Morin.API.Data;

public class SongData
{
    private readonly string _connStr;

    public SongData(string connStr)
    {
        _connStr = connStr;
    }


    #region SongList

    public async Task<int> InsertSongListAsync(SongListInfo info)
    {
        var sql = @"INSERT INTO `t_morin_song_list` ( `Id`, `Name`, `UserId`, `AddedAt`, `AddedAgent`, `AddedIP`) 
                                                      VALUES 
                                                    ( @Id, @Name, @UserId, @AddedAt, @AddedAgent, @AddedIP);";

        var param = new DynamicParameters();
        param.Add("Id", info.Id, DbType.UInt64);
        param.Add("Name", info.Name, DbType.String, size: 100);
        param.Add("UserId", info.UserId, DbType.UInt64);
        param.Add("AddedAt", info.AddedAt, DbType.Int64);
        param.Add("AddedAgent", info.AddedAgent, DbType.String, size: 500);
        param.Add("AddedIP", info.AddedIP, DbType.String, size: 100);

        using var conn = new MySqlConnection(_connStr);
        return await conn.ExecuteAsync(sql.ToString(), param);
    }

    public async Task<int> UpdateSongListNameAsync(ulong songListId, string name)
    {
        var sql = @$" UPDATE `t_morin_song_list` SET `Name` = @Name WHERE `Id` = {songListId}; ";
        var param = new DynamicParameters();
        param.Add("Name", name, DbType.String, size: 100);

        using var conn = new MySqlConnection(_connStr);
        return await conn.ExecuteAsync(sql, param);
    }

    public async Task<bool> DeleteSongListAsync(ulong[] songListIds)
    {
        var sql = $"DELETE FROM `t_morin_song_list` WHERE `Id` IN @Ids; ";
        var sqlDetail = @$" DELETE FROM `t_morin_song_list_detail` WHERE `SongListId` IN @Ids; ";

        using var conn = new MySqlConnection(_connStr);
        await conn.OpenAsync();
        var trans = await conn.BeginTransactionAsync();

        try
        {
            await conn.ExecuteAsync(sql, new { Ids = songListIds }, trans);
            await conn.ExecuteAsync(sqlDetail, new { Ids = songListIds }, trans);
            trans.Commit();
            return true;
        }
        catch (Exception)
        {
            trans.Rollback();
            return false;
        }
    }

    public async Task<IEnumerable<SongListInfo>> GetSongListBySongListIdAsync(ulong songListId)
    {
        var sql = @$"SELECT `Id`, `Name`, `UserId`, `AddedAt`, `AddedAgent`, `AddedIP` FROM `t_morin_song_list` WHERE `Id` = {songListId}; ";

        using var conn = new MySqlConnection(_connStr);
        return await conn.QueryAsync<SongListInfo>(sql);
    }

    public async Task<IEnumerable<SongListInfo>> GetSongListByUserIdAsync(ulong userId)
    {
        var sql = @$"SELECT    `Id`, `Name`, `UserId`, `AddedAt`, `AddedAgent`, `AddedIP` FROM `t_morin_song_list` WHERE `UserId` = {userId}; ";

        using var conn = new MySqlConnection(_connStr);
        return await conn.QueryAsync<SongListInfo>(sql);
    }

    #endregion

    #region SongListDetail

    public async Task<int> CollectSongAsync(SongListDetailInfo info)
    {
        var sql = @"INSERT INTO `t_morin_song_list_detail` ( `Id`, `UserId`, `SongListId`, `Platform`, `Song`, `SongId`, `Singer`, `SingerId`, `Album`, `AlbumId`, `Duration`, `HasMv`, `HasLoss`, `ImgUrl`, `LrcUrl`, `MP3Url`, `AddedAt`, `AddedIP`, `AddedAgent`) 
                                                             VALUES 
                                                           ( @Id, @UserId, @SongListId, @Platform, @Song, @SongId, @Singer, @SingerId, @Album, @AlbumId, @Duration, @HasMv, @HasLoss, @ImgUrl, @LrcUrl, @MP3Url, @AddedAt, @AddedIP, @AddedAgent);";

        var param = new DynamicParameters();
        param.Add("Id", info.Id, DbType.UInt64);
        param.Add("UserId", info.UserId, DbType.Int64);
        param.Add("SongListId", info.SongListId, DbType.UInt64);
        param.Add("Platform", info.Platform, DbType.Byte);
        param.Add("Song", info.Song, DbType.String, size: 100);
        param.Add("SongId", info.SongId, DbType.String, size: 50);
        param.Add("Singer", info.Singer, DbType.String, size: 50);
        param.Add("SingerId", info.SingerId, DbType.UInt32);
        param.Add("Album", info.Album, DbType.String, size: 50);
        param.Add("AlbumId", info.AlbumId, DbType.UInt32);
        param.Add("Duration", info.Duration, DbType.UInt32);
        param.Add("HasMv", info.HasMv, DbType.Byte);
        param.Add("HasLoss", info.HasLoss, DbType.Byte);
        param.Add("ImgUrl", info.ImgUrl, DbType.String, size: 500);
        param.Add("LrcUrl", info.LrcUrl, DbType.String, size: 500);
        param.Add("MP3Url", info.MP3Url, DbType.String, size: 500);
        param.Add("AddedAt", info.AddedAt, DbType.Int64);
        param.Add("AddedIP", info.AddedIP, DbType.String, size: 100);
        param.Add("AddedAgent", info.AddedAgent, DbType.String, size: 500);

        using var conn = new MySqlConnection(_connStr);
        return await conn.ExecuteAsync(sql.ToString(), param);
    }

    public async Task<int> UnCollectSongAsync(ulong detailId)
    {
        var sql = @$"DELETE FROM `t_morin_song_list_detail` WHERE `Id` = {detailId}";

        using var conn = new MySqlConnection(_connStr);
        return await conn.ExecuteAsync(sql);
    }

    public async Task<IEnumerable<SongListDetailInfo>> GetSongListDetailBySongListIdAsync(ulong songListId)
    {
        var sql = @$"SELECT `Id`, `UserId`, `SongListId`, `Platform`, `Song`, `SongId`, `Singer`, `SingerId`, `Album`, `AlbumId`, `Duration`, `HasMv`, `HasLoss`, `ImgUrl`, `LrcUrl`, `MP3Url`
                     FROM `t_morin_song_list_detail` WHERE `SongListId` = {songListId};";

        using var conn = new MySqlConnection(_connStr);
        return await conn.QueryAsync<SongListDetailInfo>(sql);
    }

    #endregion

    #region SongLog

    public async Task<int> InsertSongLogAsync(SongLogInfo info)
    {
        var sql = "INSERT INTO `t_morin_song_log` ( `Id`, `UserId`, `SongId`, `AddedAt`, `AddedAgent`, `AddedIP`) VALUES ( @Id, @UserId, @SongId, @AddedAt, @AddedAgent, @AddedIP);";

        var param = new DynamicParameters();
        param.Add("Id", info.Id, DbType.UInt64);
        param.Add("UserId", info.UserId, DbType.UInt64);
        param.Add("SongId", info.SongId, DbType.UInt64);
        param.Add("AddedAt", info.AddedAt, DbType.Int64);
        param.Add("AddedAgent", info.AddedAgent, DbType.String, size: 500);
        param.Add("AddedIP", info.AddedIP, DbType.String, size: 100);

        using var conn = new MySqlConnection(_connStr);
        return await conn.ExecuteAsync(sql, param);
    }

    #endregion

}