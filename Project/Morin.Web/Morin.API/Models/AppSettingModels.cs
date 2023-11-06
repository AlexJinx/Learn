namespace Morin.API.Models;

public class MorinConfigModel
{
    public string JwtIss { get; set; }

    public string JwtAud { get; set; }

    public string JwtSecret { get; set; }

    public int JwtExpireMinutes { get; set; }

    public string MySQLConn { get; set; }

    public string MySQLConnWrite { get; set; } = "server=101.34.61.66;port=3306;database=morin;uid=root;pwd=123456;charset=utf8mb4";

    public string NeedSwagger { get; set; }

    public string Port { get; set; }
}