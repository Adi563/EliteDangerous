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
                    var starSystem = ConvertCsvLineToStarSystem(line);

                    starSystems.Add(starSystem);
                }
            }

            return starSystems;
        }

        public static System.Collections.Generic.IEnumerable<StarSystem> GetAllSystemsHighMemory(string filePath)
        {
            var lines = System.IO.File.ReadAllLines(filePath, System.Text.Encoding.ASCII);

            var starSystems = lines.Except(new string[] { lines[0] }).AsParallel().Select(line =>
            {
                return ConvertCsvLineToStarSystem(line);
            });

            return starSystems.Where(s => s != null);
        }

        public static StarSystem ConvertCsvLineToStarSystem(string line)
        {
            var lineElements = line.Split(new char[] { ',' });

            var id = uint.Parse(lineElements[0]);

            uint indexOfLastNameElement = 2;
            while (!lineElements[indexOfLastNameElement].EndsWith("\""))
            { indexOfLastNameElement++; }

            string[] nameElements = new string[indexOfLastNameElement - 1];
            System.Array.Copy(lineElements, 2, nameElements, 0, nameElements.Length);
            var name = string.Join(",", nameElements);

            var x = float.Parse(lineElements[3 + nameElements.Length - 1]);
            var y = float.Parse(lineElements[4 + nameElements.Length - 1]);
            var z = float.Parse(lineElements[5 + nameElements.Length - 1]);

            return new StarSystem { Id = id, Name = name, Location = new System.Numerics.Vector3(x, y, z) };
        }
    }
}