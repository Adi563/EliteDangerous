namespace EliteDangerous.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MsSqlTest
    {
        MsSqlHandler msSqlHandler = new MsSqlHandler(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=elitedangerous;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        [TestMethod]
        public void GetSystems()
        {
            msSqlHandler.GetSystems();
        }

        [TestMethod]
        public void GetSystemsScoopable()
        {
            var systemsScoopable = msSqlHandler.GetSystemsScoopable();
        }

        [TestMethod]
        public void InsertSystems()
        {
            var systems = CsvHandler.GetAllSystems(ReadCsvTest.CSV_FILE_PATH);

            msSqlHandler.InsertSystems(systems);
        }

        [TestMethod]
        public void DeleteSystems()
        {
            msSqlHandler.DeleteSystems();
        }

        [TestMethod]
        public void UpdateSystemsScoopable()
        {
            const string JSON_FILE_PATH = @"C:\Users\Adrian\Downloads\Elite\bodies.jsonl";
            using (var stream = new System.IO.FileStream(JSON_FILE_PATH, System.IO.FileMode.Open))
            {
                var systemIdsScoopable = JsonHandler.GetSystemIdsByMainStarClasses(stream, new string[] { "O", "B", "A", "F", "G", "K", "M" }).ToArray();
                msSqlHandler.UpdateSystemsScoopable(systemIdsScoopable, true);
            }
        }

        [TestMethod]
        public void UpdateSystemsUnScoopable()
        {
            const string JSON_FILE_PATH = @"C:\Users\Adrian\Downloads\Elite\bodies.jsonl";
            using (var stream = new System.IO.FileStream(JSON_FILE_PATH, System.IO.FileMode.Open))
            {
                var systemIdsScoopable = JsonHandler.GetSystemIdsByMainStarClasses(stream, new string[] { "L", "TTS", "C", "Y", "T", "DC", "DA", "S", "DQ", "W", "DAZ", "D", "WC", "MS", "AEBE", "WO", "WN", "WNC", "CN", "DB", "DAB", "DCV", "DAV", "CJ", "DBV", "DBZ" }).ToArray();
                msSqlHandler.UpdateSystemsScoopable(systemIdsScoopable, false);
            }
        }
    }
}