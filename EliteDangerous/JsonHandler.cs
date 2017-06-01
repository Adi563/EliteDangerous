namespace EliteDangerous
{
    using System.Linq;

    public class JsonHandler
    {
        /// <summary>
        /// Gets the system ids by main star classes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="starClass">The star class.</param>
        /// <returns></returns>
        public static System.Collections.Generic.IEnumerable<uint> GetSystemIdsByMainStarClasses(System.IO.Stream stream, System.Collections.Generic.IEnumerable<string> starClass)
        {
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


        /// <summary>
        /// Gets the star classes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static System.Collections.Generic.List<string> GetStarClasses(System.IO.Stream stream)
        {
            var starClasses = new System.Collections.Generic.List<string>();
            
            using (var reader = new System.IO.StreamReader(stream))
            {
                string line;
                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    var jObject = Newtonsoft.Json.Linq.JObject.Parse(line);
                    var isMainStar = jObject.Value<bool?>("is_main_star");
                    if (!(isMainStar.HasValue && isMainStar.Value)) { continue; }

                    var spectralClass = jObject.Value<string>("spectral_class");

                    if (!starClasses.Contains(spectralClass))
                    { starClasses.Add(spectralClass); }
                }
            }

            return starClasses;
        }
    }
}
