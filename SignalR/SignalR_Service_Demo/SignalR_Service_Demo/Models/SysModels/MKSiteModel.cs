namespace SignalR_Service_Demo.Models.SysModels;

public class MKSiteModel
{
    public string Name { get; set; }

    public string Company { get; set; }

    public string ICP { get; set; }

    public string JwtIss { get; set; }

    public string JwtAud { get; set; }

    public string JwtPri { get; set; }

    public string JwtPub { get; set; }

    public int JwtExpireMinutes { get; set; }

    public string RedisConn { get; set; }

    public string MySQLConn { get; set; }

    public int ClientDownloadDays { get; set; }

    public string FileRoot { get; set; }

    public string FileUrl { get; set; }

    public string JobRunAt { get; set; }
}