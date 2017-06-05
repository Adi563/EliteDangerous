namespace EliteDangerous
{
    using System.Linq;

    public class MySqlHandler
    {
        private static MySql.Data.MySqlClient.MySqlConnection connection;

        static MySqlHandler()
        {
            var stringBuilder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder();
            stringBuilder.Server = "127.0.0.1";
            stringBuilder.Port = 3307;
            stringBuilder.UserID = "root";
            stringBuilder.Password = "";
            stringBuilder.Database = "elitedangerous";

            connection = new MySql.Data.MySqlClient.MySqlConnection(stringBuilder.ConnectionString);
            connection.Open();
        }

        public static System.Collections.Generic.IEnumerable<StarSystem> GetSystems()
        {
            return ReadSystems("1 = 1").ToArray();
        }

        public static System.Collections.Generic.IEnumerable<StarSystem> GetSystemsScoopable()
        {
            return ReadSystems("Scoopable = 1").ToArray();
        }

        public static StarSystem GetSystem(string name)
        {
            return ReadSystems(string.Format("Name = '{0}' LIMIT 1", name)).First();
        }

        public static System.Collections.Generic.IEnumerable<StarSystem> ReadSystems(string where)
        {
            var command = connection.CreateCommand();
            command.CommandText = string.Format("SELECT * FROM Systems WHERE {0}", where);
            command.CommandTimeout = 1000;

            using (var reader = command.ExecuteReader())
            {
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
        }


        /// <summary>
        /// Inserts the systems.
        /// </summary>
        /// <param name="starSystems">The star systems.</param>
        public static void InsertSystems(System.Collections.Generic.IEnumerable<StarSystem> starSystems)
        {
            int currentItem = -1;
            uint currentGroup = 0;
            var systemsGrouped = starSystems.GroupBy(system =>
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


        /// <summary>
        /// Deletes the systems.
        /// </summary>
        public static void DeleteSystems()
        {
            var commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText = "TRUNCATE TABLE Systems";
            commandUpdate.ExecuteNonQuery();
        }


        /// <summary>
        /// Updates the systems scoopable.
        /// </summary>
        /// <param name="systemIds">The system ids.</param>
        /// <param name="scoopable">if set to <c>true</c> [scoopable].</param>
        public static void UpdateSystemsScoopable(System.Collections.Generic.IEnumerable<uint> systemIds, bool scoopable)
        {
            uint number = 0;
            uint group = 0;
            var systemIdsScoopableGrouped = systemIds.GroupBy(id =>
            {
                number++;

                if (number > 100000)
                { number = 0; group++; }

                return group;
            });

            foreach (var systemIdScoopableGrouped in systemIdsScoopableGrouped)
            {
                var systemIdsScoopableString = string.Join(",", systemIdScoopableGrouped);

                var commandUpdate = connection.CreateCommand();
                commandUpdate.CommandText = string.Format("UPDATE Systems SET Scoopable = {0} WHERE Id IN ({1})", scoopable ? "1" : "0", systemIdsScoopableString);
                commandUpdate.CommandTimeout = 1000;
                commandUpdate.ExecuteNonQuery();
            }
        }
    }
}
