namespace EliteDangerous.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ReadJsonTest
    {
        [TestMethod]
        public void ReadAllBodies()
        {
            using (var stream = new System.IO.FileStream(@"C:\Users\Adrian\Downloads\elite\bodies.jsonl", System.IO.FileMode.Open))
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
    }
}