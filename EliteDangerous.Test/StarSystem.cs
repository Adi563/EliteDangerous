namespace EliteDangerous.Test
{
    public class StarSystem
    {
        public string Name { get; set; }
        public System.Numerics.Vector3 Location { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
