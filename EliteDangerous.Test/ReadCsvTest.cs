namespace EliteDangerous.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ReadCsvTest
    {
        [TestMethod]
        public void ReadAllSystems()
        {
            GetAllSystems();
        }

        public static System.Collections.Generic.IEnumerable<StarSystem> GetAllSystems()
        {
            var starSystems = new System.Collections.Generic.List<StarSystem>();

            using (var stream = new System.IO.FileStream(@"C:\Users\Adrian\Downloads\elite dangerous systems.csv", System.IO.FileMode.Open))
            {
                var streamReader = new System.IO.StreamReader(stream);
                streamReader.ReadLine();

                string line = string.Empty;
                while (!string.IsNullOrEmpty(line = streamReader.ReadLine()))
                {
                    var lineElements = line.Split(new char[] { ',' });

                    if (lineElements.Length != 28) { continue; }

                    var name = lineElements[2];

                    var x = float.Parse(lineElements[3]);
                    var y = float.Parse(lineElements[4]);
                    var z = float.Parse(lineElements[5]);

                    starSystems.Add(new StarSystem { Name = name, Location = new System.Numerics.Vector3(x, y, z) });
                }
            }

            return starSystems;
        }

        [TestMethod]
        public void TestMethod1()
        {
            var systemNames = new string[] { "v374 pegasi" };
            var starSystems = new System.Collections.Generic.List<StarSystem>();

            using (var stream = new System.IO.FileStream(@"C:\Users\Adrian\Downloads\elite dangerous systems.csv", System.IO.FileMode.Open))
            {
                var streamReader = new System.IO.StreamReader(stream);

                string line = string.Empty;
                while (!string.IsNullOrEmpty(line = streamReader.ReadLine()))
                {
                    var lineElements = line.Split(new char[] { ',' });

                    var name = lineElements[2].Replace("\"", string.Empty).ToLower();

                    if (!systemNames.Contains(name)) { continue; }

                    var x = float.Parse(lineElements[3]);
                    var y = float.Parse(lineElements[4]);
                    var z = float.Parse(lineElements[5]);

                    starSystems.Add(new StarSystem { Name = name, Location = new System.Numerics.Vector3(x, y, z) });

                    if (starSystems.Count == systemNames.Length) { break; }
                }
            }
        }
    }
}