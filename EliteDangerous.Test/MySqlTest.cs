namespace EliteDangerous.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MySqlTest
    {
        [TestMethod]
        public void GetSystems()
        {
            MySqlHandler.GetSystems();
        }

        [TestMethod]
        public void GetSystemsScoopable()
        {
            var systemsScoopable = MySqlHandler.GetSystemsScoopable();
        }

        [TestMethod]
        public void InsertSystems()
        {
            var systems = ReadCsvTest.GetAllSystems();

            //MySqlHandler.InsertSystems(systems);
        }

        [TestMethod]
        public void DeleteSystems()
        {
            MySqlHandler.DeleteSystems();
        }

        [TestMethod]
        public void UpdateSystemsScoopable()
        {
            const string JSON_FILE_PATH = @"C:\Users\Adrian\Downloads\Elite\bodies.jsonl";
            using (var stream = new System.IO.FileStream(JSON_FILE_PATH, System.IO.FileMode.Open))
            {
                var systemIdsScoopable = JsonHandler.GetSystemIdsByMainStarClasses(stream, new string[] { "O", "B", "A", "F", "G", "K", "M" }).ToArray();
                MySqlHandler.UpdateSystemsScoopable(systemIdsScoopable, true);
            }
        }

        [TestMethod]
        public void UpdateSystemsUnScoopable()
        {
            const string JSON_FILE_PATH = @"C:\Users\Adrian\Downloads\Elite\bodies.jsonl";
            using (var stream = new System.IO.FileStream(JSON_FILE_PATH, System.IO.FileMode.Open))
            {
                var systemIdsScoopable = JsonHandler.GetSystemIdsByMainStarClasses(stream, new string[] { "L", "TTS", "C", "Y", "T", "DC", "DA", "S", "DQ", "W", "DAZ", "D", "WC", "MS", "AEBE", "WO", "WN", "WNC", "CN", "DB", "DAB", "DCV", "DAV", "CJ", "DBV", "DBZ" }).ToArray();
                MySqlHandler.UpdateSystemsScoopable(systemIdsScoopable, false);
            }
        }
    }
}