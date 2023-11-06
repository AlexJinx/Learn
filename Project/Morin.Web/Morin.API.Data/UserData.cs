using Dapper;

using Morin.API.Entity;

using MySql.Data.MySqlClient;

using System.Data;

namespace Morin.API.Data;

public class UserData
{
    private readonly string _connStr;

    public UserData(string connStr)
    {
        _connStr = connStr;
    }


    public async Task<int> InsertAsync(UserInfo info)
    {
        var sql = @"INSERT INTO `t_morin_user` ( `Id`, `UserName`, `Account`, `Password`, `PasswordSalt`, `Gender`, `Birthday`, `AddedAt`, `AddedAgent`, `AddedIP`) 
                                                 VALUES 
                                               ( @Id, @UserName, @Account, @Password, @PasswordSalt, @Gender, @Birthday, @AddedAt, @AddedAgent, @AddedIP);";

        var param = new DynamicParameters();
        param.Add("Id", info.Id, DbType.UInt64);
        param.Add("UserName", info.UserName, DbType.String, size: 100);
        param.Add("Account", info.Account, DbType.String, size: 11);
        param.Add("Password", info.Password, DbType.String, size: 32);
        param.Add("PasswordSalt", info.PasswordSalt, DbType.String, size: 64);
        param.Add("Gender", info.Gender, DbType.Byte);
        param.Add("Birthday", info.Birthday, DbType.StringFixedLength, size: 8);
        param.Add("AddedAt", info.AddedAt, DbType.Int64);
        param.Add("AddedAgent", info.AddedAgent, DbType.String, size: 500);
        param.Add("AddedIP", info.AddedIP, DbType.String, size: 100);

        using var conn = new MySqlConnection(_connStr);
        return await conn.ExecuteAsync(sql.ToString(), param);
    }

    public async Task<int> InsertLoginLogAsync(ulong id, ulong by, long at, string agent, string ip)
    {
        var sql = "INSERT INTO `t_morin_user_login_log` (`Id`, `AddedBy`, `AddedAt`, `AddedAgent`, `AddedIP`) ";
        sql += "VALUES (@Id, @AddedBy, @AddedAt, @AddedAgent, @AddedIP);";

        var param = new DynamicParameters();
        param.Add("Id", id, DbType.UInt64);
        param.Add("AddedBy", by, DbType.UInt64);
        param.Add("AddedAt", at, DbType.Int64);
        param.Add("AddedAgent", agent, DbType.String, size: 500);
        param.Add("AddedIP", ip, DbType.String, size: 100);

        using var conn = new MySqlConnection(_connStr);
        return await conn.ExecuteAsync(sql, param);
    }

    public async Task<int> UpdatePwdAsync(ulong userId, string pwd, string pwdSalt)
    {
        var sql = @$" UPDATE `t_morin_user` SET `Password` = @Pwd, `PasswordSalt` = @PwdSalt WHERE `Id` = {userId}; ";

        var param = new DynamicParameters();
        param.Add("Pwd", pwd, DbType.StringFixedLength, size: 32);
        param.Add("PwdSalt", pwdSalt, DbType.StringFixedLength, size: 64);

        using var conn = new MySqlConnection(_connStr);
        return await conn.ExecuteAsync(sql, param);
    }

    public async Task<UserInfo> GetUserByAccount(string account)
    {
        var sql = "SELECT `Id`, `UserName`, `Account`, `Password`, `PasswordSalt`, `Gender`, `Birthday`, `AddedAt` FROM `t_morin_user` WHERE `Account` = @Account;";

        var param = new DynamicParameters();
        param.Add("Account", account, DbType.String, size: 11);

        using var conn = new MySqlConnection(_connStr);
        return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql, param);
    }

    public async Task<UserInfo> GetUserInfoAsync(ulong userId)
    {
        var sql = @$"SELECT `Id`, `UserName`, `Account`, `Password`, `PasswordSalt`, `Gender`, `Birthday`, `AddedAt`, `AddedAgent`, `AddedIP` 
                         FROM `t_morin_user` WHERE `Id` = {userId}";

        using var conn = new MySqlConnection(_connStr);
        return await conn.QueryFirstOrDefaultAsync<UserInfo>(sql);
    }

    public async Task<IEnumerable<UserInfo>> GetUsersInfoAsync(ulong[] userIds)
    {
        var sql = @" SELECT  `Id`, `UserName`, `Account`, `Password`, `PasswordSalt`, `Gender`, `Birthday`, `AddedAt`, `AddedAgent`, `AddedIP`  FROM `t_morin_user`  WHERE `Id` IN @Ids; ";

        using var conn = new MySqlConnection(_connStr);
        return await conn.QueryAsync<UserInfo>(sql.ToString(), new { Ids = userIds });
    }

    public async Task<(IEnumerable<UserInfo> Result, long Cnt)> GetInfosByPageAsync(int offset, int pageSize)
    {
        var sql = @$" SELECT  `Id`, `UserName`, `Account`, `Password`, `PasswordSalt`, `Gender`, `Birthday`, `AddedAt`, `AddedAgent`, `AddedIP`
                         FROM `t_morin_user`
                         ORDER BY AddedAt DESC  LIMIT {offset}, {pageSize};
                         SELECT COUNT(1) AS `Cnt` FROM `t_morin_user` ";

        using var conn = new MySqlConnection(_connStr);
        var results = await conn.QueryMultipleAsync(sql.ToString());
        var users = await results.ReadAsync<UserInfo>();
        var cnt = await results.ReadFirstOrDefaultAsync<long>();
        results.Dispose();
        return ValueTuple.Create(users, cnt);
    }
}

