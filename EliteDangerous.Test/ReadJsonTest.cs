namespace EliteDangerous.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ReadJsonTest
    {
        public const string JSON_FILE_PATH = @"C:\Users\Adrian\Downloads\Elite\bodies.jsonl";

        [TestMethod]
        public void GetStarClassesTest()
        {
            using (var stream = new System.IO.FileStream(JSON_FILE_PATH, System.IO.FileMode.Open))
            {
                var starClasses = JsonHandler.GetStarClasses(stream);
                System.Diagnostics.Debug.WriteLine(string.Join(",", starClasses.Select(s => string.Format("\"{0}\"", s))));
            }
        }

        [TestMethod]
        public void GetSystemIdsByMainStarClassesTest()
        {
            using (var stream = new System.IO.FileStream(JSON_FILE_PATH, System.IO.FileMode.Open))
            {
                var systemIds = JsonHandler.GetSystemIdsByMainStarClasses(stream, new string[] { "F" }).ToArray();
            }
        }
    }
}