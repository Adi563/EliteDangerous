namespace EliteDangerous.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ReadCsvTest
    {
        public const string CSV_FILE_PATH = @"C:\Adi\systems.csv";

        [TestMethod]
        public void GetAllSystems()
        {
            CsvHandler.GetAllSystems(CSV_FILE_PATH);
        }

        [TestMethod]
        public void ConvertCsvLineToStarSystem()
        {
            CsvHandler.ConvertCsvLineToStarSystem("109117,1231564,\"WDS J05353 - 0522Ja Jb\",605.3125,-439.25,-1092.46875,0,0,176,None,5,None,80,None,16,Low,10,None,,,,0,1491336391,\"PSH 136\",,,,");
        }
    }
}