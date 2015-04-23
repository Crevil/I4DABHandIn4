using System.Collections.Generic;

namespace DAL.Entities
{
    public class Location
    {
        public int LocationId { get; set; }
        public int Floor { get; set; }
        public int Number { get; set; }
        public int Size { get; set; }

        public ICollection<Measurement> Measurements { get; set; }

        public Location()
        {
            
        }

        public Location(int floor, int number, int size)
        {
            Floor = floor;
            Number = number;
            Size = size;
        }
    }
}
