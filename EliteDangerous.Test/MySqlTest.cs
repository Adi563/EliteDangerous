﻿namespace EliteDangerous.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MySqlTest
    {
        MySqlHandler msSqlHandler = new MySqlHandler("localhost", 3307, "root", "k83Wd3/5*");

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
            using (var stream = new System.IO.FileStream(ReadJsonTest.JSON_FILE_PATH, System.IO.FileMode.Open))
            {
                var systemIdsScoopable = JsonHandler.GetSystemIdsByMainStarClasses(stream, new string[] { "O", "B", "A", "F", "G", "K", "M" }).ToArray();
                msSqlHandler.UpdateSystemsScoopable(systemIdsScoopable, true);
            }
        }

        [TestMethod]
        public void UpdateSystemsUnScoopable()
        {
            using (var stream = new System.IO.FileStream(ReadJsonTest.JSON_FILE_PATH, System.IO.FileMode.Open))
            {
                var systemIdsScoopable = JsonHandler.GetSystemIdsByMainStarClasses(stream, new string[] { "L", "TTS", "C", "Y", "T", "DC", "DA", "S", "DQ", "W", "DAZ", "D", "WC", "MS", "AEBE", "WO", "WN", "WNC", "CN", "DB", "DAB", "DCV", "DAV", "CJ", "DBV", "DBZ" }).ToArray();
                msSqlHandler.UpdateSystemsScoopable(systemIdsScoopable, false);
            }
        }
    }
}