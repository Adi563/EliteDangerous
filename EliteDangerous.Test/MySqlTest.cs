namespace EliteDangerous.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MySqlTest
    {
        private static MySql.Data.MySqlClient.MySqlConnection connection;

        static MySqlTest()
        {
            var stringBuilder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder();
            stringBuilder.Server = "192.168.10.157";
            stringBuilder.UserID = "root";
            stringBuilder.Password = "";
            stringBuilder.Database = "elitedangerous";

            connection = new MySql.Data.MySqlClient.MySqlConnection(stringBuilder.ConnectionString);
            connection.Open();
        }

        [TestMethod]
        public void GetSystems()
        {
            ReadSystems().ToArray();
        }

        public static System.Collections.Generic.IEnumerable<StarSystem> ReadSystems()
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Systems";
            command.CommandTimeout = 1000;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = (uint)reader["Id"];
                var name = (string)reader["Name"];
                var x = (float)reader["X"];
                var y = (float)reader["Y"];
                var z = (float)reader["Z"];

                yield return new StarSystem { Id = id, Name = name, Location = new System.Numerics.Vector3(x, y, z) };
            }
        }

        [TestMethod]
        public void InsertSystems()
        {
            var systems = ReadCsvTest.GetAllSystems();

            int currentItem = -1;
            uint currentGroup = 0;
            var systemsGrouped = systems.GroupBy(system =>
            {
                currentItem++;

                if (currentItem >= 100000)
                { currentItem = 0; currentGroup++; }

                return currentGroup;
            }).ToArray();
            
            const string queryPattern = "INSERT INTO Systems VALUES {0};";
            foreach (var systemGroup in systemsGrouped)
            {
                string queryValues = string.Join(",", systemGroup.Select(system => string.Format("({0},'{1}',{2},{3},{4})", system.Id, system.Name.Replace("\"", string.Empty).Replace("'", "''"), system.Location.X, system.Location.Y, system.Location.Z)));
                string query = string.Format(queryPattern, queryValues);
                var commandInsert = connection.CreateCommand();
                commandInsert.CommandText = query;
                commandInsert.CommandTimeout = 1000;
                commandInsert.ExecuteNonQuery();
            }
        }

        [TestMethod]
        public void InsertOrUpdateSystems()
        {
            var systems = ReadCsvTest.GetAllSystems();

            foreach (var system in systems)
            {
                var command = connection.CreateCommand();
                command.CommandText = string.Format("SELECT COUNT(*) FROM Systems WHERE Id = {0}", system.Id);
                var numberOfRows = (long)command.ExecuteScalar();

                if (numberOfRows > 0)
                {
                    var commandUpdate = connection.CreateCommand();
                    commandUpdate.CommandText = string.Format("UPDATE Systems SET Name = '{0}', X = {1}, Y = {2}, Z = {3} WHERE Id = {4}", system.Name.Replace("\"", string.Empty).Replace("'", "''"), system.Location.X, system.Location.Y, system.Location.Z, system.Id);
                    commandUpdate.ExecuteNonQuery();
                }
                else
                {
                    var commandInsert = connection.CreateCommand();
                    commandInsert.CommandText = string.Format("INSERT INTO Systems (Id, Name, X, Y, Z) VALUES ({0}, '{1}', {2}, {3}, {4})", system.Id, system.Name.Replace("\"", string.Empty).Replace("'", "''"), system.Location.X, system.Location.Y, system.Location.Z);
                    commandInsert.ExecuteNonQuery();
                }
            }
        }

        [TestMethod]
        public void DeleteSystems()
        {
            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText = "TRUNCATE TABLE Systems";
            commandUpdate.ExecuteNonQuery();
        }
    }
}