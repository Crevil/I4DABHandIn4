﻿using System.Collections.Generic;

namespace DAL.Entities
{
    public class Appartment
    {
        public int AppartmentId { get; set; }
        public int Floor { get; set; }
        public int No { get; set; }
        public double Size { get; set; }

        public ICollection<Measurement> Measurements { get; set; }

        public Appartment()
        {
            
        }

        public Appartment(int floor, int number, int size)
        {
            Floor = floor;
            No = number;
            Size = size;
        }
    }
}
