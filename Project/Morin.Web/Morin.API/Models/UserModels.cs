using Morin.API.Entity;

namespace Morin.API.Models;


#region Base

public class UserTokenModel
{
    public ulong UserId { get; set; }

    public string UserName { get; set; }

    public string Account { get; set; }

    public string Token { get; set; }

    public int Expire { get; set; }
}

public class CurrentUserModel : UserInfo
{
    public string UserAgent { get; set; }

    public string RemoteIP { get; set; }
}

#endregion

#region Model

public class UserLoginModel
{
    public string Account { get; set; }

    public string Password { get; set; }
}

public class RegInModel
{
    public string UserName { get; set; }

    public string Account { get; set; }

    public string Password { get; set; }

    public string RePassword { get; set; }

    public GenderEnum Gender { get; set; }

    public string Birthday { get; set; }
}

#endregion

