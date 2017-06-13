namespace EliteDangerous
{
    using System.Linq;

    public class MySqlHandler : DatabaseHandler
    {
        public MySqlHandler(string serverName, uint port, string userName, string password) : base(GetConnection(serverName, port, userName, password))
        { }

        public static MySql.Data.MySqlClient.MySqlConnection GetConnection(string serverName, uint port, string userName, string password)
        {
            var stringBuilder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder();
            stringBuilder.Server = serverName;
            stringBuilder.Port = port;
            stringBuilder.UserID = userName;
            stringBuilder.Password = password;
            stringBuilder.Database = "elitedangerous";

            var connection = new MySql.Data.MySqlClient.MySqlConnection(stringBuilder.ConnectionString);

            return connection;
        }
    }
}
