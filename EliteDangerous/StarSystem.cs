﻿namespace EliteDangerous
{
    public class StarSystem
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public System.Numerics.Vector3 Location { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
