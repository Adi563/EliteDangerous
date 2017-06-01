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
            var systemIdsScoopable = ReadJsonTest.GetSystemIdsByMainStarClasses(new string[] { "O", "B", "A", "F", "G", "K", "M" }).ToArray();

            MySqlHandler.UpdateSystemsScoopable(systemIdsScoopable, true);
        }

        [TestMethod]
        public void UpdateSystemsUnScoopable()
        {
            var systemIdsScoopable = ReadJsonTest.GetSystemIdsByMainStarClasses(new string[] { "L", "TTS", "C", "Y", "T", "DC", "DA", "S", "DQ", "W", "DAZ", "D", "WC", "MS", "AEBE", "WO", "WN", "WNC", "CN", "DB", "DAB", "DCV", "DAV", "CJ", "DBV", "DBZ" }).ToArray();

            MySqlHandler.UpdateSystemsScoopable(systemIdsScoopable, false);
        }
    }
}