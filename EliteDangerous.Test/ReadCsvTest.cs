namespace EliteDangerous.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ReadCsvTest
    {
        const string CSV_FILE_PATH = @"C:\Users\Adrian\Downloads\Elite\elite dangerous systems.csv";
        const string CSV_FILE_PATH2 = @"C:\Users\Adrian\Downloads\Elite\scoopable sytems.csv";

        [TestMethod]
        public void ReadAllSystems()
        {
            GetAllSystems();
        }

        public static System.Collections.Generic.IList<StarSystem> ReadScoopableSystems()
        {
            var starSystems = new System.Collections.Generic.List<StarSystem>();

            using (var stream = new System.IO.FileStream(CSV_FILE_PATH2, System.IO.FileMode.Open))
            {
                var streamReader = new System.IO.StreamReader(stream);
                streamReader.ReadLine();

                string line = string.Empty;
                while (!string.IsNullOrEmpty(line = streamReader.ReadLine()))
                {
                    var lineElements = line.Split(new char[] { ';' });

                    var name = lineElements[0];

                    var x = float.Parse(lineElements[1]);
                    var y = float.Parse(lineElements[2]);
                    var z = float.Parse(lineElements[3]);

                    starSystems.Add(new StarSystem { Name = name, Location = new System.Numerics.Vector3(x, y, z) });
                }
            }

            return starSystems;
        }

        public static System.Collections.Generic.IEnumerable<StarSystem> GetAllSystems()
        {
            var starSystems = new System.Collections.Generic.List<StarSystem>();

            using (var stream = new System.IO.FileStream(CSV_FILE_PATH, System.IO.FileMode.Open))
            {
                var streamReader = new System.IO.StreamReader(stream);
                streamReader.ReadLine();

                string line = string.Empty;
                while (!string.IsNullOrEmpty(line = streamReader.ReadLine()))
                {
                    var lineElements = line.Split(new char[] { ',' });

                    if (lineElements.Length != 28) { continue; }

                    var id = uint.Parse(lineElements[0]);
                    var name = lineElements[2];

                    var x = float.Parse(lineElements[3]);
                    var y = float.Parse(lineElements[4]);
                    var z = float.Parse(lineElements[5]);

                    starSystems.Add(new StarSystem { Id = id, Name = name, Location = new System.Numerics.Vector3(x, y, z) });
                }
            }

            return starSystems;
        }

        [TestMethod]
        public void WriteSystemsCsvWithScoopableMainStars()
        {
            var starSystems = GetAllSystems();
            var systemIds = ReadJsonTest.GetSystemIdsByMainStarClasses(new string[] { "O", "B", "A", "F", "G", "K", "M" }).ToArray();

            var starSystemsFiltered = starSystems.AsParallel().Where(ss => systemIds.Contains(ss.Id)).ToArray();
            
            using (var stream = new System.IO.FileStream(CSV_FILE_PATH2, System.IO.FileMode.Create))
            {
                var streamWriter = new System.IO.StreamWriter(stream, System.Text.Encoding.UTF8);

                foreach (var starSystem in starSystemsFiltered)
                {
                    streamWriter.WriteLine("{0};{1};{2};{3}", starSystem.Name.Replace("\"", string.Empty), starSystem.Location.X, starSystem.Location.Y, starSystem.Location.Z);
                }

                streamWriter.Flush();
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var systemNames = new string[] { "v374 pegasi" };
            var starSystems = new System.Collections.Generic.List<StarSystem>();

            using (var stream = new System.IO.FileStream(CSV_FILE_PATH, System.IO.FileMode.Open))
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