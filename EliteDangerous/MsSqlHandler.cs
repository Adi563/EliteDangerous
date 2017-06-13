namespace EliteDangerous
{
    using System.Linq;

    public class MsSqlHandler : DatabaseHandler
    {
        public MsSqlHandler(string serverName, string userName, string password) : base(GetConnection(serverName, userName, password))
        { }

        public MsSqlHandler(string connectionString) : base(GetConnection(connectionString))
        { }

        public static System.Data.SqlClient.SqlConnection GetConnection(string connectionString)
        {
            var connection = new System.Data.SqlClient.SqlConnection(connectionString);

            return connection;
        }

        public static System.Data.SqlClient.SqlConnection GetConnection(string serverName, string userName, string password)
        {
            var stringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            stringBuilder.DataSource = serverName;
            stringBuilder.UserID = userName;
            stringBuilder.Password = password;
            stringBuilder.InitialCatalog = "elitedangerous";

            var connection = new System.Data.SqlClient.SqlConnection(stringBuilder.ConnectionString);

            return connection;
        }
    }
}
