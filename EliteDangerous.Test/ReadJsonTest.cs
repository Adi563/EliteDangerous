namespace EliteDangerous.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ReadJsonTest
    {
        const string JSON_FILE_PATH = @"C:\Users\Adrian\Downloads\Elite\bodies.jsonl";

        [TestMethod]
        public void ReadAllMainStars()
        {
            using (var stream = new System.IO.FileStream(JSON_FILE_PATH, System.IO.FileMode.Open))
            using (var reader = new System.IO.StreamReader(stream))
            {
                string line;
                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    var jObject = Newtonsoft.Json.Linq.JObject.Parse(line);
                    var isMainStar = jObject.Value<bool?>("is_main_star");
                    if (!(isMainStar.HasValue && isMainStar.Value)) { continue; }


                    var name = jObject.Value<string>("name");
                    var spectralClass = jObject.Value<string>("spectral_class");
                    var systemId = jObject.Value<uint>("system_id");

                    System.Diagnostics.Debug.WriteLine("{0}: {1}", name, spectralClass);
                }
            }
        }

        [TestMethod]
        public void GetSystemIdsByMainStarClassesTest()
        {
            var systemIds = GetSystemIdsByMainStarClasses(new string[] { "F" }).ToArray();
        }

        public static System.Collections.Generic.IEnumerable<uint> GetSystemIdsByMainStarClasses(System.Collections.Generic.IEnumerable<string> starClass)
        {
            using (var stream = new System.IO.FileStream(JSON_FILE_PATH, System.IO.FileMode.Open))
            using (var reader = new System.IO.StreamReader(stream))
            {
                string line;
                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    var jObject = Newtonsoft.Json.Linq.JObject.Parse(line);
                    var isMainStar = jObject.Value<bool?>("is_main_star");
                    if (!(isMainStar.HasValue && isMainStar.Value)) { continue; }
                    
                    var spectralClass = jObject.Value<string>("spectral_class");

                    if (!starClass.Contains(spectralClass))
                    { continue; }

                    var systemId = jObject.Value<uint>("system_id");

                    yield return systemId;
                }
            }
        }
    }
}