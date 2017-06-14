namespace EliteDangerous
{
    using System.Linq;

    public class CsvHandler
    {
        public static System.Collections.Generic.IEnumerable<StarSystem> GetAllSystems(string filePath)
        {
            var starSystems = new System.Collections.Generic.List<StarSystem>();

            using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
            {
                var streamReader = new System.IO.StreamReader(stream, System.Text.Encoding.ASCII);
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

        public static System.Collections.Generic.IEnumerable<StarSystem> GetAllSystemsHighMemory(string filePath)
        {
            var lines = System.IO.File.ReadAllLines(filePath, System.Text.Encoding.ASCII);

            var starSystems = lines.Except(new string[] { lines[0] }).AsParallel().Select(line =>
            {
                var lineElements = line.Split(new char[] { ',' });

                if (lineElements.Length != 28) { return null; }

                var id = uint.Parse(lineElements[0]);
                var name = lineElements[2];

                var x = float.Parse(lineElements[3]);
                var y = float.Parse(lineElements[4]);
                var z = float.Parse(lineElements[5]);

                return new StarSystem { Id = id, Name = name, Location = new System.Numerics.Vector3(x, y, z) };
            });

            return starSystems.Where(s => s != null);
        }
    }
}
