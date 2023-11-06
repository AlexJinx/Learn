using MK.Common;

using Morin.API.Data;
using Morin.API.Entity;

namespace Morin.API.Business;

public class UserBusiness
{
    private readonly UserData _userData;
    public UserBusiness(string connStr)
    {
        _userData = new UserData(connStr);
    }

    public async Task<MKResult> InsertAsync(UserInfo info)
    {
        try
        {
            var result = await _userData.InsertAsync(info);
            if (result > 0)
            {
                return new MKResult(msg: "注册成功");
            }

            return new MKResult(400, "注册失败");
        }
        catch (Exception e)
        {
            return new MKResult(500, e.Message, e);
        }
    }

    public async Task<MKResult> InsertLoginLogAsync(ulong id, ulong by, long at, string agent, string ip)
    {
        try
        {
            var result = await _userData.InsertLoginLogAsync(id, by, at, agent, ip);
            if (result > 0)
            {
                return new MKResult(msg: "记录登录日志成功");
            }

            return new MKResult(400, "记录登录日志失败");
        }
        catch (Exception e)
        {
            return new MKResult(500, e.Message, e);
        }
    }

    public async Task<MKResult> UpdatePwdAsync(ulong userId, string pwd, string pwdSalt)
    {
        try
        {
            var result = await _userData.UpdatePwdAsync(userId, pwd, pwdSalt);
            if (result > 0)
            {
                return new MKResult(200, "修改成功");
            }
            return new MKResult(400, "修改失败");
        }
        catch (Exception e)
        {
            return new MKResult(code: 500, msg: e.Message, ex: e);
        }
    }

    public async Task<MKResult<UserInfo>> GetUserCountByAccount(string account)
    {
        try
        {
            var count = await _userData.GetUserByAccount(account);
            return new MKResult<UserInfo>(count, count == null ? 0 : 1);
        }
        catch (Exception e)
        {
            return new MKResult<UserInfo>(code: 500, msg: e.Message, ex: e);
        }
    }

    public async Task<MKResult<UserInfo>> GetUserInfoAsync(ulong userId)
    {
        try
        {
            var result = await _userData.GetUserInfoAsync(userId);
            return new MKResult<UserInfo>(result, result == null ? 0 : 1);
        }
        catch (Exception e)
        {
            return new MKResult<UserInfo>(code: 500, msg: e.Message, ex: e);
        }
    }

    public async Task<MKResult<IEnumerable<UserInfo>>> GetUsersInfoAsync(ulong[] ids)
    {
        try
        {
            var results = await _userData.GetUsersInfoAsync(ids);
            return new MKResult<IEnumerable<UserInfo>>(results, results.Count());
        }
        catch (Exception e)
        {
            return new MKResult<IEnumerable<UserInfo>>(code: 500, msg: e.Message, ex: e);
        }
    }

    public async Task<MKResult<IEnumerable<UserInfo>>> GetInfosByPageAsync(int pageIdx, int pageSize)
    {
        try
        {
            var (Result, Cnt) = await _userData.GetInfosByPageAsync(pageIdx * pageSize, pageSize);
            return new MKResult<IEnumerable<UserInfo>>(Result, Cnt);
        }
        catch (Exception e)
        {
            return new MKResult<IEnumerable<UserInfo>>(code: 500, msg: e.Message, ex: e);
        }
    }
}

